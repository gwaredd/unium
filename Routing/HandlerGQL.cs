// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using gw.proto.utils;
using gw.gql;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //

    public static class HandlerGQL
    {
        // allow queries to be cached

        class CachedQuery
        {
            public Query    Q;
            public bool     Reselect;
        }

        public static object CacheContext( RequestAdapter req, string path, int level )
        {
            var cache = new CachedQuery() { Q = new Query( path, Unium.Root ), Reselect = true };

            if( level > 0 )
            {
                cache.Q.Select();
                cache.Reselect = false;
            }

            return cache;
        }


        //----------------------------------------------------------------------------------------------------

        public static void Query( RequestAdapter req, string path )
        {
            // if we have already cached the query then use that
            
            if( req.CachedContext != null )
            {
                var cache = req.CachedContext as CachedQuery;
                var query = cache.Q;

                if( cache.Reselect )
                {
                    query.Select();
                    req.Respond( JsonReflector.Reflect( query.Execute() ) );
                    query.Clear();
                }
                else
                {
                    req.Respond( JsonReflector.Reflect( query.Execute() ) );
                }
            }

            // otherwise execute it

            else
            {
                var query = new Query( path, Unium.Root ).Select();
                req.Respond( JsonReflector.Reflect( query.Execute() ) );
            }
        }
    }
}

#endif
