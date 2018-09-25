// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;

using gw.proto.utils;

namespace gw.proto.http
{
    // https://tools.ietf.org/html/rfc6455

    public enum WebSocketOpCode : byte
    {
        Continue    = 0x00,
        Text        = 0x01,
        Binary      = 0x02,
        Close       = 0x08,
        Ping        = 0x09,
        Pong        = 0x0A,
    }


    /// <summary>
    /// decodes the input stream a turns it into a websocket frame for processing
    /// </summary>

    public class WebSocketFrame
    {
        public static readonly int MaxFrameHeaderSize = 16; // assuming no extensions


        public byte[]   Payload;
        public bool     IsFinal         { get; private set; }
        public bool     IsMasked        { get; private set; }
        public bool     IsComplete      { get { return mState == Reading.Complete; } }
        public WebSocketOpCode OpCode   { get; private set; }


        //------------------------------------------------------------------------------

        enum Reading
        {
            Header,
            Payload,
            Complete
        }

        Reading mState          = Reading.Header;
        byte[]  mMask           = new byte[ 4 ];
        uint    mPayloadLen     = 0;
        uint    mWritePosition  = 0;

        public WebSocketFrame()
        {
            Payload     = null;
            OpCode      = WebSocketOpCode.Continue;
            IsFinal     = false;
            IsMasked    = false;
        }


        //------------------------------------------------------------------------------
        /// <summary>
        /// reads bytes from an input stream until we have a complete websocket frame.
        /// will throw an exception on an invalid frame
        /// </summary>
        /// <param name="buffer">byte[] buffer of bytes on the input stream</param>
        /// <param name="offset">offset into that stream/array to start reading from</param>
        /// <param name="available">number of bytes available to read from the offset</param>
        /// <returns>>number of bytes consumber from the buffer (may be 0)</returns>

        public uint Read( byte[] buffer, uint offset, uint available )
        {
            if( mState == Reading.Header )
            {
                return ReadHeader( buffer, offset, available );
            }
            else if( mState == Reading.Payload )
            {
                return ReadPayload( buffer, offset, available );
            }

            return 0;
        }


        //------------------------------------------------------------------------------
        // read header
        //
        //
        // 0                   1                   2                   3
        // 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        // +-+-+-+-+-------+-+-------------+-------------------------------+
        // |F|R|R|R| opcode|M| Payload len |    Extended payload length    |
        // |I|S|S|S|  (4)  |A|     (7)     |             (16/64)           |
        // |N|V|V|V|       |S|             |   (if payload len==126/127)   |
        // | |1|2|3|       |K|             |                               |
        // +-+-+-+-+-------+-+-------------+ - - - - - - - - - - - - - - - +
        // |     Extended payload length continued, if payload len == 127  |
        // + - - - - - - - - - - - - - - - +-------------------------------+
        // |                               |Masking-key, if MASK set to 1  |
        // +-------------------------------+-------------------------------+
        // | Masking-key (continued)       |          Payload Data         |
        // +-------------------------------- - - - - - - - - - - - - - - - +
        // :                     Payload Data continued ...                :
        // + - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
        // |                     Payload Data continued ...                |
        // +---------------------------------------------------------------+
        //
        //

        [Flags]
        enum Flag : byte
        {
            Fin     = 0x80, // end of data
            Rsv     = 0x70, // reserved bits
            Op      = 0x0F, // op code
            Mask    = 0x80, // mask
            Payload = 0x7F, // payload portion
        }


        uint ReadHeader( byte[] buffer, uint offset, uint numBytesAvailable )
        {
            // we need at least the first 2 bytes of the message frame to be able to decode the header size

            if( numBytesAvailable < 2 )
            {
                return 0;
            }


            // read first two bytes

            var codeByte    = buffer[ offset + 0 ];
            var payloadByte = buffer[ offset + 1 ];
                

            // calculate header size

            IsMasked = (byte)( payloadByte & (byte) Flag.Mask ) == (byte) Flag.Mask;

            uint payload    = (uint) ( payloadByte & (byte) Flag.Payload );
            uint headerSize = 2;

            if( IsMasked )
            {
                headerSize += 4;
            }

            if( payload == 126 )
            {
                headerSize += 2;
            }
            else if( payload == 127 )
            {
                headerSize += 8;
            }


            // wait until we have the whole header available
            // ... this saves trying to track state, but does mean we need at least enough buffer space for the whole header

            if( numBytesAvailable < headerSize )
            {
                return 0;
            }


            // we have the whole header, so get the length of the payload

            if( payload < 126 )
            {
                mPayloadLen = (uint) payload;
            }
            else if( payload == 126 )
            {
                var size = Endian.NetworkToHost( BitConverter.ToUInt16( buffer, (int) offset + 2 ) );

                if( size > WebSocket.MaxPayload )
                {
                    throw new WebSocketException( WebSocketStatus.PayloadTooLarge );
                }

                mPayloadLen = (uint) size;
            }
            else // payload == 127
            {
                var size = Endian.NetworkToHost( BitConverter.ToUInt64( buffer, (int) offset + 2 ) );

                if( size > WebSocket.MaxPayload )
                {
                    throw new WebSocketException( WebSocketStatus.PayloadTooLarge );
                }

                mPayloadLen = (uint) size;
            }
                

            // decode all the control bits

            IsFinal = ( codeByte & (byte) Flag.Fin ) == (byte) Flag.Fin;
            OpCode  = (WebSocketOpCode) ( codeByte & (byte) Flag.Op );

            if( ( codeByte & (byte) Flag.Rsv ) != 0 )
            {
                throw new WebSocketException( WebSocketStatus.ProtocolError );
            }


            // read mask key

            if( IsMasked )
            {
                Array.Copy( buffer, offset + headerSize - 4, mMask, 0, 4 );
            }


            // if no payload for this message then we're done (presumably this is a control frame)

            if( mPayloadLen == 0 )
            {
                mState = Reading.Complete;
                return headerSize;
            }

            // otherwise start reading the payload body

            mState  = Reading.Payload;
            Payload = new byte[ mPayloadLen ];


            // return total number of bytes consumed from the buffer

            var numBytesOfBodyRead = ReadPayload( buffer, offset + headerSize, numBytesAvailable - headerSize );

            return headerSize + numBytesOfBodyRead;
        }


        //------------------------------------------------------------------------------

        uint ReadPayload( byte[] buffer, uint offset, uint numBytesAvailable )
        {
            uint numBytes = Math.Min( numBytesAvailable, mPayloadLen - mWritePosition );

            if( IsMasked )
            {
                for( uint i=0; i < numBytes; i++, mWritePosition++ )
                {
                    Payload[ mWritePosition ] = (byte) ( buffer[ offset++ ] ^ mMask[ mWritePosition % 4 ] );
                }
            }
            else
            {
                Array.Copy( buffer, offset, Payload, mWritePosition, numBytes );
                mWritePosition += numBytes;
            }

            if( mWritePosition >= mPayloadLen )
            {
                mState = Reading.Complete;
            }

            return numBytes;
        }
    }
}

#endif
