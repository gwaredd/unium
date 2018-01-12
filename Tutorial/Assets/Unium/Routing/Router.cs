// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;

using gw.proto.http;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Route
    {
        public delegate void RouteHandler( RequestAdapter req, string path );
        public delegate object CacheHandler( RequestAdapter req, string path, int level );

        public string           Path                    = null;
        public bool             DispatchOnGameThread    = true;
        public bool             ExactMatch              = false;
        public CacheHandler     CacheContext            = null;     // called to create cached context (in case of repeating)
        public RouteHandler     Handler                 = null;     // called to process request

        public string RelativePath( RequestAdapter request )
        {
            return Path == null ? request.Path : request.Path.Substring( Path.Length );
        }

        public void SetCacheContext( RequestAdapter request, int level )
        {
            request.CachedContext = CacheContext == null ? null : CacheContext( request, RelativePath( request ), level );
        }

        public void Dispatch( RequestAdapter request )
        {
            try
            {
                Handler( request, RelativePath( request ) );
            }
            catch( Exception e )
            {
                request.Reject( ResponseCode.BadRequest );
                UniumComponent.Error( e.Message );
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Router
    {
        public Route Otherwise = null;


        //----------------------------------------------------------------------------------------------------

        bool        mSorted = false;
        List<Route> mRoutes = new List<Route>();

        public Route Find( string url )
        {
            if( mSorted == false )
            {
                mRoutes.Sort( (a,b) => b.Path.Length - a.Path.Length ); // sort by reverse string length - cheesy solution!
                mSorted = true;
            }

            foreach( var route in mRoutes )
            {
                // path filter
                
                var path = route.Path;

                if( url.Length < path.Length )
                {
                    continue;
                }

                if( route.ExactMatch || url.Length == path.Length )
                {
                    if( url == route.Path )
                    {
                        return route;
                    }

                    continue;
                }

                // check we have a separator

                var last = url[ route.Path.Length ];

                if( last != '/' && last != '.' )
                {
                    continue;
                }

                // does this route match?

                if( url.StartsWith( route.Path ) )
                {
                    return route;
                }
            }

            return Otherwise;
        }


        //----------------------------------------------------------------------------------------------------

        public Route Add( string path, Route.RouteHandler handler )
        {
            var route = new Route() { Path = path, Handler = handler };

            mRoutes.Add( route );
            mSorted = false;

            return route;
        }

        public Route AddImmediate( string path, Route.RouteHandler handler )
        {
            var route = Add( path, handler );
            route.DispatchOnGameThread  = false;
            return route;
        }
            
        public void Remove( string path )
        {
            mRoutes.RemoveAll( route => route.Path == path );
        }
    }
}

