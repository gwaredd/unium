// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System.Collections.Generic;

using gw.gql;
using gw.proto.http;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // special socket commands

    public partial class UniumSocket
    {
        public class SocketCommands
        {
            UniumSocket mSocket;

            public SocketCommands( UniumSocket socket ) { mSocket = socket; }

            public void stop( string id )       { mSocket.Stop( id ); }
            public void unbind( string id )     { mSocket.Unbind( id ); }
            public void ping()                  { mSocket.Pong(); }
            public int  repeaterCount           { get { return mSocket.mRepeaters.Count; } }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class HandlerSocketCommand
    {
        public static void Execute( RequestAdapter adapter, string path )
        {
            var socket  = ( adapter as RequestAdapterSocket ).Socket;
            var root    = new Dictionary<string,object>() { { "socket", new UniumSocket.SocketCommands( socket ) } };

            var q = new Query( adapter.Path, root ).Select();
            var r = q.Execute();

            if( r.Count == 0 )
            {
                adapter.Reject( ResponseCode.NotFound );
            }
        }
    }
}

#endif
