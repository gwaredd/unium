// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

//#define GW_WEBSOCKET_DETAILED_LOGGING

using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

using gw.proto.utils;
using System.Diagnostics;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////
    // https://tools.ietf.org/html/rfc6455

    public class WebSocket
    {
        ////////////////////////////////////////////////////////////////////////////////
        // some global config options

        public static readonly ulong MaxPayload      = 2 * 1024 * 1024;  // 2Mb
        public static readonly ulong MaxMessage      = 8 * 1024 * 1024;  // 8Mb
        public static readonly int   BufferSize      = 8 * 1024;         // 8k
        public static readonly int   Heartbeat       = 30 * 1000;        // 30s
        public static readonly int   MaxFrameSize    = 128 * 1024;       // 128k
        public static readonly int   CloseTimeout    = 3 * 1000;         // 3s
        public static readonly int   PingPongTimeout = 12 * 1000;        // 12s


        ////////////////////////////////////////////////////////////////////////////////
        // public state

        public enum State
        {
            Connecting,
            Open,
            Closing,
            Closed
        }

        public uint     ID          { get; private set; }   // unique id for socket
        public string   URL         { get; private set; }   // URL associated with socket request
        public bool     IsServer    { get; private set; }   // is in server or client mode
        public State    Connection  { get; private set; }   // current connection state


        // socket events for binding

        public delegate void MessageDelegate( WebSocket ws, byte[] message, bool isText );
        public MessageDelegate  OnMessage   = null;
        public object           User        = null;


        ////////////////////////////////////////////////////////////////////////////////
        // internal state

        HttpRequest     mRequest;                                       // used only during handshake
        Stream          mStream;                                        // I/O stream
        Dispatcher      mDispatcher;


        // reading a web socket frame

        WebSocketFrame  mFrame;                                         // current frame
        byte[]          mReadBuffer;                                    // temporary input buffer
        uint            mReadPosition;                                  // position to read from in buffer
        int             mWritePosition;                                 // position to write to in buffer

        // for when reading fragmented messages

        List<byte[]>    mFragmentPayloads    = new List<byte[]>();      // list of individual payloads
        uint            mFragmentTotalBytes  = 0;                       // total size of total payload so far
        bool            mFragmentIsText      = true;                    // data type set on frist frame so keep track


        // writing data

        Queue<byte[]>   mSendQueue          = new Queue<byte[]>();

        object          mCloseLock          = new object();
        bool            mCloseSent          = false;
        bool            mCloseReceived      = false;

        // timeouts

        long            mCloseStarted       = 0;
        long            mLastRecv           = 0;
        bool            mSentPing           = false;


        //------------------------------------------------------------------------------

        public WebSocket( HttpRequest request )
        {
            mRequest    = request;
            ID          = request.ID;
            URL         = request.URL;
            mDispatcher = request.Dispatch;

            IsServer    = true;
            Connection  = State.Connecting;
        }


        ////////////////////////////////////////////////////////////////////////////////
        // for websocket handshake negotiation - Accept or Reject MUST be called by the dispatcher


        /// <summary>
        /// Accept request, complete handshake then "open" the socket
        /// </summary>

        public void Accept()
        {
            mStream = mRequest.GetStream();

            // complete WebSocket handshake

            var key     = mRequest.Header( "Sec-WebSocket-Key" ) + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            var hash    = SHA1.Create().ComputeHash( Encoding.UTF8.GetBytes( key ) );
            var accept  = Convert.ToBase64String( hash );

            var res = mRequest.Response;

            res.Response( ResponseCode.SwitchingProtocols );

            res.Headers[ "Connection" ]           = "Upgrade";
            res.Headers[ "Upgrade" ]              = "websocket";
            res.Headers[ "Sec-WebSocket-Accept" ] = accept;

            res.WriteHeaders();

            mRequest = null; // release request, we don't need it any more


            // open socket for business

            Open();
        }


        /// <summary>
        /// Reject websocket connection request and close the connection
        /// </summary>
        /// <param name="code">Code.</param>

        public void Reject( ResponseCode code = ResponseCode.NotFound )
        {
            mRequest.Reject( code );

            mRequest   = null;
            Connection = State.Closed;
        }


        //------------------------------------------------------------------------------
        // init web socket

        void Open()
        {
            Util.Print( "[ws:{0}] Opening websocket id:{0}", ID );

            mFrame          = new WebSocketFrame();
            mReadBuffer     = new byte[ BufferSize ];
            mReadPosition   = 0;
            mWritePosition  = 0;
            mLastRecv       = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Connection      = State.Open;

            mStream.BeginRead( mReadBuffer, 0, mReadBuffer.Length, OnRead, this );

            mDispatcher.SocketOpen( this );
        }


        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Optionally, call this periodically for timeouts
        /// </summary>

        public void Tick()
        {
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if( Connection == State.Open )
            {
                if( mSentPing )
                {
                    // wait for pong response

                    if( now - mLastRecv > PingPongTimeout )
                    {
                        Detail( "[ws:{0}] Pong not received, closing connection ({1},{2})", ID, now, mLastRecv );
                        Close();
                    }
                }
                else if( now - mLastRecv > Heartbeat )
                {
                    // send ping if no activity for a while
                    Ping();
                }
            }
            else if( Connection == State.Closing )
            {
                // timeout during closing

                if( now - mCloseStarted > CloseTimeout )
                {
                    CloseSocket();
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// send close frame and go to closing state
        /// </summary>
        /// <param name="code">Code.</param>

        public void Close( WebSocketStatus code = WebSocketStatus.NormalClosure )
        {
            if( Connection != State.Open )
            {
                Util.Warn( "[ws:{0}] Attempting to close a socket that is not open", ID );
                return;
            }


            Detail( "[ws:{0}] Sending close frame", ID );

            // create close payload

            var text = Encoding.UTF8.GetBytes( WebSocketUtils.CodeToString( code ) );
            var num  = BitConverter.GetBytes( Endian.HostToNetwork( (ushort) code ) );
            var data = new byte[ 2 + text.Length ];

            Array.Copy( num, 0, data, 0, 2 );
            Array.Copy( text, 0, data, 2, text.Length );

            // close

            Connection      = State.Closing;
            mCloseStarted   = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            SendAsync( data, WebSocketOpCode.Close );
        }


        //------------------------------------------------------------------------------

        void ShouldCloseSocket()
        {
            // close socket when we get to the end of both input and output stream

            lock( mCloseLock )
            {
                if( mCloseSent && mCloseReceived )
                {
                    CloseSocket();
                }
            }
        }

        void CloseSocket()
        {
            Connection = State.Closed;

            mStream.Close();
            Util.Print( "[ws:{0}] Connection closed", ID );

            mDispatcher.SocketClose( this );
        }


        ////////////////////////////////////////////////////////////////////////////////

        void Ping()
        {
            Detail( "[ws:{0}] Send ping", ID );

            mLastRecv = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            mSentPing = true;

            SendAsync( null, WebSocketOpCode.Ping );
        }

        public virtual void SendAsync( string text )
        {
            SendAsync( Encoding.UTF8.GetBytes( text ), WebSocketOpCode.Text );
        }

        public virtual void SendAsync( byte[] data, WebSocketOpCode opCode = WebSocketOpCode.Binary )
        {
            // socket must be open to send

            if( Connection != State.Open )
            {
                // except "close" special case

                if( Connection != State.Closing || opCode != WebSocketOpCode.Close )
                {
                    Util.Warn( "[ws:{0}] Can't send data as conection is not open", ID );
                    return;
                }
            }



            // break payload into fragments of max size (probably only one :)

            var totalBytes = data != null ? data.Length : 0;
            var index = 0;

            do
            {
                // create fragment header

                var numBytes    = Math.Min( MaxFrameSize, totalBytes - index );
                var next        = index + numBytes;
                var isFinal     = next >= totalBytes;


                // add opcode if first fragment

                byte code = 0x00;

                if( index == 0 )
                {
                    code |= (byte) opCode;
                }

                // flag final fragement

                if( isFinal )
                {
                    code |= 0x80;
                }


                // size

                byte[]  buffer;
                int     headerSize;

                if( numBytes < 126 )
                {
                    headerSize = 2;

                    buffer = new byte[ numBytes + headerSize ];
                    buffer[ 0 ] = code;
                    buffer[ 1 ] = (byte) numBytes;
                }
                else if( numBytes < 65536 )
                {
                    headerSize = 4;

                    buffer = new byte[ numBytes + headerSize ];
                    buffer[ 0 ] = code;
                    buffer[ 1 ] = (byte) 126;

                    Array.Copy( BitConverter.GetBytes( Endian.HostToNetwork( (ushort) numBytes ) ), 0, buffer, 2, 2 );
                }
                else
                {
                    headerSize = 10;

                    buffer = new byte[ numBytes + headerSize ];
                    buffer[ 0 ] = code;
                    buffer[ 1 ] = (byte) 127;

                    Array.Copy( BitConverter.GetBytes( Endian.HostToNetwork( (ulong) numBytes ) ), 0, buffer, 2, 8 );
                }


                // copy data

                if( data != null )
                {
                    if( IsServer )
                    {
                        Array.Copy( data, index, buffer, headerSize, numBytes );
                    }
                    else
                    {
                        buffer[ 1 ] |= (byte) 0x80; // masked

                        // clients MUST mask data sent to a server (RFC 6455/5.1)
                        throw new NotImplementedException();
                    }
                }


                //Detail( "[ws:{0}] Sending {1}/{2}", ID, index + numBytes, totalBytes ) );

                lock( mSendQueue )
                {
                    mSendQueue.Enqueue( buffer );

                    // if first message on queue then start it off

                    if( mSendQueue.Count == 1 )
                    {
                        mStream.BeginWrite( buffer, 0, buffer.Length, OnSent, this );
                    }
                }

                index += numBytes;
            }
            while( index < totalBytes );
        }


        //------------------------------------------------------------------------------

        void OnSent( IAsyncResult result )
        {
            try
            {
                mStream.EndWrite( result );

                lock( mSendQueue )
                {
                    // remove send buffer from queue

                    mSendQueue.Dequeue();

                    // send next if we have one

                    if( mSendQueue.Count > 0 )
                    {
                        var data = mSendQueue.Peek();
                        mStream.BeginWrite( data, 0, data.Length, OnSent, this );
                    }
                    else if( Connection == State.Closing )
                    {
                        lock( mCloseLock )
                        {
                            mCloseSent = true;
                        }

                        ShouldCloseSocket();
                    }
                }
            }
            catch( Exception e )
            {
                Util.Error( e.ToString() );
                CloseSocket();
            }
        }



        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// read data from network socket (callback, async read)
        /// </summary>

        void OnRead( IAsyncResult asyncResult )
        {
            try
            {
                // read bytes from stream

                var bytesRead = mStream.EndRead( asyncResult );

                if( bytesRead <= 0 )
                {
                    throw new Exception( "Abnormal termination" );
                }

                mWritePosition += bytesRead;
                mLastRecv       = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                // loop through all frames in buffer, NB: there may be more than one!

                uint bytesConsumed = 0;

                do
                {
                    bytesConsumed = mFrame.Read( mReadBuffer, mReadPosition, (uint) mWritePosition - mReadPosition );
                    mReadPosition += bytesConsumed;

                    // process a completed websocket frame

                    if( mFrame.IsComplete )
                    {
                        OnCompleteFrame();

                        if( mFrame.OpCode == WebSocketOpCode.Close )
                        {
                            break;
                        }

                        mFrame = new WebSocketFrame();
                    }
                }
                while( bytesConsumed > 0 );


                // if closing, stop reading

                if( mCloseReceived )
                {
                    ShouldCloseSocket();
                    return;
                }


                // otherwise, wait for more data ...


                // maximise space for next read by returning to beginning of buffer if no bytes remaining

                if( mWritePosition == mReadPosition )
                {
                    mWritePosition = 0;
                    mReadPosition  = 0;
                }

                // otherwise ensure we always have space for at least one whole WebSocket frame

                else if( mReadPosition > mReadBuffer.Length - WebSocketFrame.MaxFrameHeaderSize )
                {
                    var bytesLeft = mWritePosition - (int) mReadPosition;

                    Array.Copy( mReadBuffer, mReadPosition, mReadBuffer, 0, bytesLeft );

                    mWritePosition = bytesLeft;
                    mReadPosition  = 0;
                }

                // then wait for more data

                mStream.BeginRead( mReadBuffer, mWritePosition, mReadBuffer.Length - mWritePosition, OnRead, this );
            }
            catch( WebSocketException e )
            {
                // protocol error, so try and gracefully shut down the connection if we can

                Util.Error( "[ws:{0}] {1}", ID, e.Code.ToString() );

                if( Connection == State.Open )
                {
                    // send close request

                    Close( e.Code );

                    // and wait for response

                    mStream.BeginRead( mReadBuffer, mWritePosition, mReadBuffer.Length - mWritePosition, OnRead, this );
                }
                else
                {
                    // socket not in open state, so just terminate
                    HardClose( e.ToString() );
                }
            }
            catch( Exception e )
            {
                // fatal error

                Util.Error( "[ws:{0}] Fatal error - {1}", ID, e.StackTrace );
                HardClose( e.ToString() );
            }
        }

        void HardClose( string error )
        {
            Util.Error( "[ws:{0}] Terminating connection - {1}", ID, error );
            CloseSocket();
        }


        //------------------------------------------------------------------------------
        /// <summary>
        /// when we have read a complete frame from the input stream then handle it accordingly
        /// </summary>

        void OnCompleteFrame()
        {
            // RFC 6455/5.1 - client MUST mask the data sent to server, the server MUST close
            // the connection if it detects frame data that is not masked

            if( mFrame.Payload != null && mFrame.IsMasked == false )
            {
                Util.Error( "[ws:{0}] Payload from client not masked", ID );
                Close( WebSocketStatus.ProtocolError );
                return;
            }


            // handle control frames - i.e., the high bit is set

            if( ( (byte) mFrame.OpCode & 0x08 ) == 0x08 )
            {
                OnControlFrame();
                return;
            }



            // is this the final (or complete) frame?

            if( mFrame.IsFinal )
            {
                // check we can actually dispatch it!

                if( OnMessage == null )
                {
                    mFragmentIsText = false;
                    mFragmentTotalBytes = 0;
                    mFragmentPayloads.Clear();

                    Util.Warn( "[ws:{0}] No handler set for websocket messages", ID );

                    return;
                }


                // turn frame(s) into a message

                if( mFragmentTotalBytes == 0 )
                {
                    // all data contained in one frame, so easy!

                    //Detail( "[ws:{0}] Recived message with length {1}", ID, mFrame.Payload.Length ) );

                    OnMessage( this, mFrame.Payload, mFrame.OpCode == WebSocketOpCode.Text );
                }
                else
                {
                    // data spread over multiple frames, so stitch back together


                    // add last frame to list

                    mFragmentTotalBytes += (uint) mFrame.Payload.Length;

                    if( mFragmentTotalBytes > MaxMessage )
                    {
                        mFragmentIsText = false;
                        mFragmentTotalBytes = 0;
                        mFragmentPayloads.Clear();

                        throw new WebSocketException( WebSocketStatus.PayloadTooLarge );
                    }

                    mFragmentPayloads.Add( mFrame.Payload );

                    Detail( "[ws:{0}] Fragment #{1} - {2} makes {3}", ID, mFragmentPayloads.Count, mFrame.Payload.Length, mFragmentTotalBytes );
                    Detail( "[ws:{0}] Recived message with length {1} over {2} fragments", ID, mFragmentTotalBytes, mFragmentPayloads.Count );

                    // concatenate into one buffer

                    var data = new byte[ mFragmentTotalBytes ];

                    var index = 0;

                    foreach( var pl in mFragmentPayloads )
                    {
                        Array.Copy( pl, 0, data, index, pl.Length );
                        index += pl.Length;
                    }

                    mFragmentIsText = false;
                    mFragmentTotalBytes = 0;
                    mFragmentPayloads.Clear();

                    // create msg

                    OnMessage( this, data, mFragmentIsText );
                }
            }


            // not final message (and we have data)

            else if( mFrame.Payload != null )
            {
                // derive type from first frame, following frames should be marked as "continue"

                if( mFragmentTotalBytes == 0 )
                {
                    mFragmentIsText = mFrame.OpCode == WebSocketOpCode.Text;
                }


                // keep list of fragmented data for later

                mFragmentTotalBytes += (uint) mFrame.Payload.Length;

                if( mFragmentTotalBytes > MaxMessage )
                {
                    mFragmentIsText = false;
                    mFragmentTotalBytes = 0;
                    mFragmentPayloads.Clear();

                    throw new WebSocketException( WebSocketStatus.PayloadTooLarge );
                }

                mFragmentPayloads.Add( mFrame.Payload );

                Detail( "[ws:{0}] Fragment #{1} - {2} makes {3}", ID, mFragmentPayloads.Count, mFrame.Payload.Length, mFragmentTotalBytes );
            }
        }


        //------------------------------------------------------------------------------
        /// <summary>
        /// handle a protocol control frame
        /// </summary>

        void OnControlFrame()
        {
            Detail( "[ws:{0}] Recvied OpCode {1}", ID, mFrame.OpCode );

            switch( mFrame.OpCode )
            {
                case WebSocketOpCode.Ping:
                {
                    // respond to a ping by "pong-ing" the payload back (maybe null, which is fine)

                    Detail( "[ws:{0}] Send pong", ID );
                    SendAsync( mFrame.Payload, WebSocketOpCode.Pong );
                }
                break;

                case WebSocketOpCode.Pong:
                    mSentPing = false;
                    break;

                case WebSocketOpCode.Close:
                {
                    lock( mCloseLock )
                    {
                        mCloseReceived = true;
                    }

                    if( Connection == State.Open )
                    {
                        Close();
                    }
                }
                break;

                default:
                {
                    // unknown control code, must be client error or newer protocol
                    throw new WebSocketException( WebSocketStatus.ProtocolError );
                }
            }
        }

        [Conditional( "GW_WEBSOCKET_DETAILED_LOGGING" )]
        void Detail( string msg, params object[] args )
        {
            Util.Print( msg, args );
        }
    }
}

#endif
