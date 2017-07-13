// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using gw.proto.http;
using gw.proto.utils;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // 

    public static class HandlerUtils
    {
        public class DeferredRedirect
        {
            public float    Timer   = 1.0f;
            public string   URL     = "";

            public DeferredRedirect( string url )
            {
                URL = url;
            }

            public void Tick( RequestAdapter req )
            {
                Timer -= Time.deltaTime;

                if( Timer <= 0.0f )
                {
                    req.Redirect( URL );
                }
            }
        }


        //----------------------------------------------------------------------------------------------------
        // take a screen shot and return the image

        public static void Screenshot( RequestAdapter req, string path )
        {
            // handle defer
            
            if( req.CachedContext != null )
            {
                ( req.CachedContext as DeferredRedirect ).Tick( req );
                return;
            }

            
            // save screenshot
            
            var filename = Path.Combine( HandlerFile.GetPath( "persistent" ), "screenshot.png" );

            #if UNITY_5
                Application.CaptureScreenshot( filename );
            #else
                ScreenCapture.CaptureScreenshot( filename );
            #endif

            UniumComponent.Log( "Screenshot '" + filename + "'" );

            // screenshots don't happen immediately, so defer a redirect for a small amount of time

            req.Defer( new DeferredRedirect( "/file/persistent/screenshot.png?cb=" + Util.RandomString() ) );
        }


        //----------------------------------------------------------------------------------------------------
        // return the debug output

        static List<string> sLog = new List<string>();

        public static void LogMessage( string condition, string stackTrace, LogType type )
        {
            sLog.Add( condition );

            if( sLog.Count > 100 )
            {
                sLog.RemoveAt( 0 );
            }
        }

        public static void DebugOutput( RequestAdapter req, string path )
        {
            req.SetContentType( "text/plain" );
            req.Respond( string.Join( Environment.NewLine, sLog.ToArray() ) );
        }


        //----------------------------------------------------------------------------------------------------
        // 

        public static void NotFound( RequestAdapter req, string path )
        {
            req.Reject( ResponseCode.NotFound );
        }


        //----------------------------------------------------------------------------------------------------

        public static void HandlerAbout( RequestAdapter req, string path )
        {
            req.Respond( JsonReflector.Reflect( new
            {
                Unium       = Unium.Version.ToString( 2 ),
                Unity       = Application.unityVersion,
                Mono        = Environment.Version.ToString(),
                IsEditor    = Application.isEditor,
                Product     = Application.productName,
                Company     = Application.companyName,
                Version     = Application.version,
                IPAddress   = Network.player.ipAddress,
                FPS         = 1.0f / Time.smoothDeltaTime,
                RunningTime = Time.realtimeSinceStartup,
                Scene       = SceneManager.GetActiveScene().name,
            }));
        }


        //----------------------------------------------------------------------------------------------------

        public static void HandlerScene( RequestAdapter req, string path )
        {
            // if path empty then list all loaded scenes

            if( string.IsNullOrEmpty( path ) || path.Length <= 1 )
            {
                var scenes = new List<object>();

                for( int i = 0; i < SceneManager.sceneCount; i++ )
                {
                    var scene = SceneManager.GetSceneAt( i );

                    scenes.Add( new {
                        name    = scene.name,
                        path    = scene.path,
                        index   = scene.buildIndex,
                        loaded  = scene.isLoaded
                    } );
                }

                req.Respond( JsonReflector.Reflect( scenes.ToArray() ) );
            }

            // otherwise load scene

            else
            {
                var name = path.Substring( 1 );
                SceneManager.LoadScene( name );
                req.Respond( string.Format( @"{{""loading"":""{0}""}}", name ) );
            }
        }


        //----------------------------------------------------------------------------------------------------

        public static void HandlerStatus( RequestAdapter req, string path )
        {
            req.Respond( JsonReflector.Reflect( new
            {
                FPS         = 1.0f / Time.smoothDeltaTime,
                RunningTime = Time.realtimeSinceStartup,
                Scene       = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            }));
        }
    }
}

#endif
