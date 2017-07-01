// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using gw.proto.http;
using gw.proto.utils;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // handle static file serving

    public static class HandlerFile
    {
        private static Dictionary<string,string> sPaths = new Dictionary<string, string>();
        
        private static IDictionary<string, string> sExtToMimeType = new Dictionary<string, string>( StringComparer.InvariantCultureIgnoreCase )
        {
            { ".bmp",   "image/bmp" },
            { ".cs",    "text/plain" },
            { ".gif",   "image/gif" },
            { ".ico",   "image/x-icon" },
            { ".jpe",   "image/jpeg" },
            { ".jpeg",  "image/jpeg" },
            { ".jpg",   "image/jpeg" },
            { ".js",    "application/x-javascript" },
            { ".json",  "application/json" },
            { ".png",   "image/png" },
            { ".tif",   "image/tiff" },
            { ".tiff",  "image/tiff" },
            { ".txt",   "text/plain" },
            { ".map",   "text/plain" },
            { ".htm",   "text/html" },
            { ".html",  "text/html" },
            { ".css",   "text/css" },
            { ".csv",   "text/csv" },
            { ".gz",    "application/x-gzip" },
            { ".zip",   "application/x-zip-compressed" },
            { ".xml",   "text/xml" },
            { ".mpg",   "video/mpeg" },
            { ".mpeg",  "video/mpeg" },
            { ".wav",   "audio/wav" },
        };

        public static string GetMimeType( string filename )
        {
            var ext = Path.GetExtension( filename );
            return !String.IsNullOrEmpty( ext ) && sExtToMimeType.ContainsKey( ext ) ? sExtToMimeType[ ext ] : "application/octet-stream";
        }

        public static void Mount( string name, string path )
        {
            sPaths[ name ] = path;
        }

        public static string GetPath( string name )
        {
            return sPaths.ContainsKey( name ) ? sPaths[ name ] : null;
        }


        //----------------------------------------------------------------------------------------------------

        public static void Serve( RequestAdapter req, string path )
        {
            try
            {
                // find "mouted" drive

                if( path[0] == '/' )
                {
                    path = path.Substring( 1 );
                }

                var drive = sPaths[ "root" ];

                foreach( var key in sPaths.Keys )
                {
                    if( path.StartsWith( key + "/" ) )
                    {
                        drive = sPaths[ key ];
                        path  = path.Substring( key.Length + 1 );
                        break;
                    }
                }

                var filepath = Path.Combine( drive, path );

                //

                if( req.Body == null )
                {
                    if( ( File.GetAttributes( filepath ) & FileAttributes.Directory ) == FileAttributes.Directory )
                    {
                        var files = from c in Directory.GetFileSystemEntries( filepath ) select Path.GetFileName( c );
                        req.Respond( JsonReflector.Reflect( files.ToArray() ) );
                    }
                    else
                    {
                        req.SetContentType( GetMimeType( filepath ) );
                        req.Respond( File.ReadAllBytes( filepath ) );
                    }
                }
                else
                {
                    try
                    {
                        File.WriteAllBytes( filepath, req.Body );
                        req.Reject( ResponseCode.OK );
                    }
                    catch( Exception )
                    {
                        req.Reject( ResponseCode.InternalServerError );
                    }
                }
            }
            catch( Exception )
            {
                req.Reject( ResponseCode.NotFound );
            }
        }        
    }
}

#endif
