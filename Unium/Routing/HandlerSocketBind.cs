// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Text.RegularExpressions;

using gw.gql;
using gw.proto.http;

namespace gw.unium
{
    public static class HandlerSocketBind
    {
        #if NET_2_0
            static Regex sMatchQuery = new Regex( @"/.*\.\w+$", RegexOptions.Compiled );
        #else
            static Regex sMatchQuery = new Regex( @"/.*\.\w+$" );
        #endif

        public static void HandleBind( RequestAdapter req, string path )
        {
            if( sMatchQuery.IsMatch( path ) == false )
            {
                req.Reject( ResponseCode.BadRequest );
                return;
            }
                        
            // select events

            var query = new Query( path + "=event", Unium.Root ).Select();


            // bind

            var bound   = 0;
            var target  = query.SearchPath.Target;
            var adapter = req as RequestAdapterSocket;

            foreach( var obj in query.Selected )
            {
                try
                {
                    var eventInfo = obj.GetType().GetEvent( target );

                    if( eventInfo != null )
                    {
                        adapter.Socket.Bind( adapter.Message, obj, eventInfo );
                        bound++;
                    }
                }
                catch( Exception e )
                {
                    UniumComponent.Warn( string.Format( "Failed to get bind to event '{0}' - {1}", target, e.Message ) );
                }
            }


            // not found if none bound

            if( bound == 0 )
            {
                req.Reject( ResponseCode.NotFound );
                return;
            }
        }
    }
}

#endif
