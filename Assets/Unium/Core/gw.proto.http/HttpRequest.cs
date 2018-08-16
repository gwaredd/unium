// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using gw.proto.utils;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////

    public class HttpRequest
    {
        // general config options

        public static int LimitPostSize     = 0;
        public static int HeaderBufferSize  = 8 * 1024;


        // request

        public Dictionary<string,string> Headers { get; private set; }

        public uint         ID              { get; private set; }
        public string       Method          { get; private set; }
        public string       URL             { get; private set; }
        public string       QueryParameters { get; private set; }
        public byte[]       Body;

        public HttpResponse Response        { get; private set; }

        public Dispatcher   Dispatch        { get; private set; }

        private Stream      mStream;


        static uint sNextID = 0;

        public HttpRequest()
        {
            ID      = sNextID++;
            Headers = new Dictionary<string,string>();
        }

        public HttpRequest( Client client )
            : this()
        {
        }


        //------------------------------------------------------------------------------
        // convenience functions

        public Stream GetStream()
        {
            return mStream;
        }

        public string Header( string key )
        {
            return Headers.ContainsKey( key ) ? Headers[ key ] : "";
        }

        public void Send( string data )
        {
            Response.Send( data );
        }

        public void Send( byte[] data )
        {
            Response.Send( data );
        }

        public void Reject( ResponseCode code )
        {
            Response.Reject( code );
        }

        public void Redirect( string url )
        {
            Response.Redirect( url );
        }


        //------------------------------------------------------------------------------
        /// <summary>
        /// Given an input stream, parse the incoming HTTP request
        /// </summary>
        /// <param name="stream">stream of bytes</param>
        /// <param name="dispatcher">dispatcher which will receive the HttpRequest</param>

        public void Process( Stream stream, Dispatcher dispatcher )
        {
            try
            {
                // setup

                mStream         = stream;
                Dispatch        = dispatcher;
                Response        = new HttpResponse( mStream );

                // read

                mBufferPos      = 0;
                mBufferedBytes  = 0;
                mBuffer         = new byte[ HeaderBufferSize ];

                ReadRequest();
                ReadHeaders();


                // process

                switch( Method )
                {
                    case "GET":

                        if( IsUpgradeRequest() )
                        {
                            DispatchWebSocket();
                        }
                        else
                        {
                            DispatchRequest();
                        }

                        break;


                    case "POST":
                        ReadBody();
                        DispatchRequest();
                        break;


                    case "OPTIONS":
                        Response.Headers[ "Access-Control-Allow-Origin"  ] = "*";
                        Response.Headers[ "Access-Control-Allow-Headers" ] = "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With";
                        Response.Close();
                        break;


                    default:
                        throw new HttpResponseException( ResponseCode.MethodNotAllowed );
                }
            }
            catch( HttpResponseException e )
            {
                Util.Warn( "[{0}] {1} {2}", ID, e.Code, HttpUtils.CodeToString( e.Code ) );
                Response.Reject( e.Code );
            }
            catch( System.IO.IOException e )
            {
                Util.Warn( "[{0}] failed to read from stream - {1}", ID, e.Message );
                mStream.Close();
            }
            catch( Exception e )
            {
                Util.Error( "[{0}] {1}", ID, e.ToString() );
                Response.Abort();
            }

            mBuffer = null;
        }


        //------------------------------------------------------------------------------

        void DispatchWebSocket()
        {
            if( Dispatch.OnSocketRequest == null )
            {
                throw new HttpResponseException( ResponseCode.NotFound );
            }

            Dispatch.OnSocketRequest( new WebSocket( this ) );
        }

        void DispatchRequest()
        {
            if( Dispatch.OnWebRequest == null )
            {
                throw new HttpResponseException( ResponseCode.NotFound );
            }

            Dispatch.OnWebRequest( this );
        }


        //------------------------------------------------------------------------------

        bool IsUpgradeRequest()
        {
            // check for connection upgrade headers

            if( Header( "Connection" ) != "Upgrade" )
            {
                return false;
            }

            if( Header( "Upgrade" ) != "websocket" )
            {
                return false;
            }

            // check websocket version

            if( Header( "Sec-WebSocket-Version" ) != "13" )
            {
                Response.Headers[ "Sec-WebSocket-Version" ] = "13";
                throw new HttpResponseException( ResponseCode.UpgradeRequired );
            }

            return true;
        }


        //------------------------------------------------------------------------------
        // get next line from input stream (buffered)

        byte[]  mBuffer         = null;
        int     mBufferPos      = 0;
        int     mBufferedBytes  = 0;

        string ReadLine()
        {
            int start = mBufferPos;

            do
            {
                while( mBufferPos < mBufferedBytes )
                {
                    var ch = mBuffer[ mBufferPos ];

                    if( ch == (byte) '\n' )
                    {
                        var end = mBufferPos++;

                        if( end > start && mBuffer[ end - 1 ] == '\r' )
                        {
                            --end;
                        }

                        return Encoding.ASCII.GetString( mBuffer, start, end - start );
                    }

                    ++mBufferPos;
                }

                // end of buffer, then 418

                if( mBufferPos == mBuffer.Length )
                {
                    throw new HttpResponseException( ResponseCode.EntityTooLarge );
                }

                // otherwise read more bytes

                int bytesRead = mStream.Read( mBuffer, mBufferPos, mBuffer.Length - mBufferPos );

                if( bytesRead <= 0 )
                {
                    // end of stream - shouldn't happen reading headers ...
                    throw new HttpResponseException( ResponseCode.RequestTimeout );
                }

                mBufferedBytes += bytesRead;
            }
            while( true );
        }


        //------------------------------------------------------------------------------

        void ReadRequest()
        {
            var line = ReadLine();

            if( String.IsNullOrEmpty( line ) )
            {
                throw new HttpResponseException( ResponseCode.BadRequest );
            }


            // parse request line

            var request = line.Split( new char[]{ ' ' }, 4, StringSplitOptions.RemoveEmptyEntries );

            if( request.Length != 3 )
            {
                throw new HttpResponseException( ResponseCode.BadRequest );
            }

            var url = request[ 1 ];
            var idx = url.IndexOf( '?' );

            Method  = request[ 0 ];
            URL     = idx >= 0 ? url.Substring( 0, idx ) : url;
            QueryParameters = idx >= 0 ? url.Substring( idx + 1 ) : string.Empty;
        }


        //------------------------------------------------------------------------------

        void ReadHeaders()
        {
            var split = new char[] { ':' };

            var line = ReadLine();

            while( string.IsNullOrEmpty( line ) == false )
            {
                var kv = line.Split( split, 2, StringSplitOptions.RemoveEmptyEntries );

                if( kv.Length != 2 )
                {
                    throw new HttpResponseException( ResponseCode.BadRequest );
                }

                // add header

                Headers[ kv[0].Trim() ] = kv[1].Trim();

                line = ReadLine();
            }
        }


        //------------------------------------------------------------------------------

        void ReadBody()
        {
            // NB: for simplicity, we're not supporting Transfer-Encoding or Content-Encoding (we're a bad HTTP/1.1 server :)

            if( Headers.ContainsKey( "Content-Encoding" ) )
            {
                // TODO: support deflate & gzip

                if( Headers[ "Content-Encoding" ] != "identity" )
                {
                    throw new HttpResponseException( ResponseCode.Unimplemented );
                }
            }

            if( Headers.ContainsKey( "Transfer-Encoding" ) )
            {
                // TODO: support chunked transfer encoding

                throw new HttpResponseException( ResponseCode.Unimplemented );
            }


            // get content length

            if( Headers.ContainsKey( "Content-Length" ) == false )
            {
                throw new HttpResponseException( ResponseCode.LengthRequired );
            }

            int numBytesToRead;

            if( int.TryParse( Headers[ "Content-Length" ], out numBytesToRead ) == false )
            {
                throw new HttpResponseException( ResponseCode.BadRequest );
            }

            if( numBytesToRead < 0 )
            {
                throw new HttpResponseException( ResponseCode.BadRequest );
            }

            if( numBytesToRead == 0 )
            {
                return; // nothing to read
            }

            if( LimitPostSize > 0 && numBytesToRead > LimitPostSize )
            {
                throw new HttpResponseException( ResponseCode.EntityTooLarge );
            }


            // create buffer for data

            Body = new byte[ numBytesToRead ];


            // copy any remaining buffered bytes

            var remaining = Math.Min( numBytesToRead, mBufferedBytes - mBufferPos );

            if( remaining > 0 )
            {
                Array.Copy( mBuffer, mBufferPos, Body, 0, remaining );
                numBytesToRead -= remaining;
            }

            mBuffer = null; // free buffer, we don't need it anymore


            // read rest from the stream

            while( numBytesToRead > 0 )
            {
                var numRead = mStream.Read( Body, Body.Length - numBytesToRead, numBytesToRead );

                if( numRead <= 0 )
                {
                    throw new HttpResponseException( ResponseCode.RequestTimeout );
                }

                numBytesToRead -= numRead;
            }
        }
    }
}

#endif
