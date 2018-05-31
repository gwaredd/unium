// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using gw.proto.http;
using gw.proto.utils;

using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //

    public static class HandlerUtils
    {
        //----------------------------------------------------------------------------------------------------
        // take a screen shot and return the image

        public static void Screenshot( RequestAdapter req, string path )
        {
            UniumComponent.Singleton.StartCoroutine( TakeScreenshot( req, path ) );
        }

        static IEnumerator TakeScreenshot( RequestAdapter req, string path )
        {
            // save screenshot

            var filename = Path.Combine( HandlerFile.GetPath( "persistent" ), "screenshot.png" );

            #if UNITY_5
                Application.CaptureScreenshot( filename );
            #else
                ScreenCapture.CaptureScreenshot( filename );
            #endif

            UniumComponent.Log( "Screenshot '" + filename + "'" );


            // screen shots don't happen immediately, so wait for a bit

            yield return new WaitForSeconds( 1.0f );


            // then redirect to the file

            req.Redirect( "/file/persistent/screenshot.png?cb=" + Util.RandomString() );
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
                IPAddress   = gw.gql.UniumUtils.GetIPAddress().ToString(),
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

#if UNITY_EDITOR

                foreach( var scene in EditorBuildSettings.scenes )
                {
                    scenes.Add( new
                    {
                        name    = Path.GetFileNameWithoutExtension( scene.path ),
                        path    = scene.path,
                        enabled = scene.enabled
                    } );
                }

#else

                for( int i = 0; i < SceneManager.sceneCount; i++ )
                {
                    var scene = SceneManager.GetSceneAt( i );

                    scenes.Add( new {
                        name    = scene.name,
                        path    = scene.path,
                        enabled = true
                    } );
                }
#endif

                req.Respond( JsonReflector.Reflect( scenes.ToArray() ) );
            }

            // otherwise load scene

            else
            {
                UniumComponent.Singleton.StartCoroutine( LoadScene( req, path.Substring( 1 ) ) );
            }
        }

        private static IEnumerator LoadScene( RequestAdapter req, string name )
        {
            var asyncOp = SceneManager.LoadSceneAsync( name, LoadSceneMode.Single );

            if( asyncOp == null )
            {
                req.Reject( ResponseCode.NotFound );
            }
            else
            {
                asyncOp.allowSceneActivation = true;

                while( asyncOp.isDone == false )
                {
                    yield return asyncOp;
                }

                req.Respond( JsonReflector.Reflect( new { scene = name }) );
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
