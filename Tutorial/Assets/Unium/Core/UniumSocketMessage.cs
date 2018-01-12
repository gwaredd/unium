// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using gw.proto.http;
using gw.proto.utils;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // represents a message received from the client and communicating back to the client through this interface

    public partial class UniumSocket
    {
        public class Message
        {
            // from json

            public class Repeat
            {
                public int      skip    = 0;
                public int      samples = int.MaxValue;
                public float    freq    = 1.0f;
                public bool     cache   = false;
            }

            public string   id;             // arbitary message identifier
            public string   q;              // query
            public Repeat   repeat;


            // replying to message

            public UniumSocket Socket;      // bound on dispatch

            public void Reply( string data )            { Socket.Send( id, "data", string.IsNullOrEmpty( data ) ? "null" : data ); }
            public void Reply( object data )            { Socket.Send( id, "data", JsonReflector.Reflect( data ) ); }
            public void Info( string msg )              { Socket.Send( id, "info", JsonTypeConverters.EscapedString( msg ) ); }
            public void Error( ResponseCode code )      { Socket.Send( id, "error", JsonTypeConverters.EscapedString( HttpUtils.CodeToString( code ) ) ); }
        }
    }
}

#endif
