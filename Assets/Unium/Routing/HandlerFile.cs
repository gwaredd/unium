// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
using UnityEngine;
using System.Threading;


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

        static void UploadFile( RequestAdapter req, string filepath )
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


        //----------------------------------------------------------------------------------------------------

        static void DownloadFileWWW( RequestAdapter req, string filepath )
        {
            var data = new WWW( filepath );

            while( !data.isDone )
            {
                Thread.Sleep( 10 );
            }

            if( string.IsNullOrEmpty( data.error ) )
            {
                req.SetContentType( GetMimeType( filepath ) );
                req.Respond( data.bytes );
            }
            else
            {
                req.Reject( ResponseCode.InternalServerError );
            }
        }


        //----------------------------------------------------------------------------------------------------

        static void DownloadFileNative( RequestAdapter req, string filepath )
        {
            // System.IO

            if( ( File.GetAttributes( filepath ) & FileAttributes.Directory ) == FileAttributes.Directory )
            {
                // list contents of directory

                var files = from c in Directory.GetFileSystemEntries( filepath ) select Path.GetFileName( c );
                req.Respond( JsonReflector.Reflect( files.ToArray() ) );
            }
            else
            {
                // dump bytes

                req.SetContentType( GetMimeType( filepath ) );
                req.Respond( File.ReadAllBytes( filepath ) );
            }
        }


        //----------------------------------------------------------------------------------------------------

        public static void Serve( RequestAdapter req, string path )
        {
            try
            {
                // remove initial

                if( path[0] == '/' )
                {
                    path = path.Substring( 1 );
                }


                // find "drive"

                var useWWW = Application.platform == RuntimePlatform.Android;
                var drive  = sPaths[ "root" ];

                foreach( var key in sPaths.Keys )
                {
                    if( path.StartsWith( key + "/" ) )
                    {
                        drive  = sPaths[ key ];
                        path   = path.Substring( key.Length + 1 );
                        useWWW = Application.platform == RuntimePlatform.Android && key != "persistent";

                        break;
                    }
                }


                // combine drive path and file path to get local path

                var filepath = Path.Combine( drive, path );

                // do the thing

                if( req.Body == null )
                {
                    if( useWWW )
                    {
                        DownloadFileWWW( req, filepath );
                    }
                    else
                    {
                        DownloadFileNative( req, filepath );
                    }
                }
                else
                {
                    UploadFile( req, filepath );
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
