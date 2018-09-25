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
            UniumComponent.Singleton.StartCoroutine( TakeScreenshot( req ) );
        }

        static IEnumerator TakeScreenshot( RequestAdapter req )
        {
            var query  = Util.ParseQueryString( req.Query );
            var width  = query[ "width" ]  != null ? Int32.Parse( query[ "width" ] )  : -1;
            var height = query[ "height" ] != null ? Int32.Parse( query[ "height" ] ) : -1;
            var scale  = query[ "scale" ]  != null ? float.Parse( query[ "scale" ], System.Globalization.CultureInfo.InvariantCulture ) : -1.0f;

            // render cameras to render texture

            int texWidth  = width > 0 ? width : Screen.width;
            int texHeight = height > 0 ? height : Screen.height;

            if( scale > 0.0f )
            {
                texWidth  = (int) ( texWidth * scale );
                texHeight = (int) ( texHeight * scale );
            }

            var screenshot = new RenderTexture( texWidth, texHeight, 24 );

            if( screenshot == null )
            {
                req.Reject( ResponseCode.InternalServerError );
                yield break;
            }

            screenshot.filterMode = FilterMode.Point; // no filtering as we are using the same resolution as the screen

            yield return new WaitForEndOfFrame();

            for( int i = 0; i < Camera.allCamerasCount; ++i )
            {
                var camera = Camera.allCameras[i];
                var previousRTex = camera.targetTexture;

                camera.targetTexture= screenshot;
                camera.Render();
                camera.targetTexture = previousRTex;
            }

            yield return null;


            // read from render texture to memory (ReadPixels will read from current active render texture)

            var pixels = new Texture2D( screenshot.width, screenshot.height, TextureFormat.RGB24, false );

            if( pixels == null )
            {
                req.Reject( ResponseCode.InternalServerError );
                yield break;
            }

            var previousActiveRTex = RenderTexture.active;
            RenderTexture.active = screenshot;

            pixels.ReadPixels( new Rect( 0, 0, screenshot.width, screenshot.height ), 0, 0 );
            pixels.Apply();

            RenderTexture.active = previousActiveRTex;

            screenshot.Release();
            UnityEngine.Object.Destroy( screenshot);

            // return png

            var bytes = pixels.EncodeToPNG();

            if( bytes != null )
            {
                req.SetContentType( "image/png" );
                req.Respond( bytes );
            }
            else
            {
                req.Reject( ResponseCode.InternalServerError );
            }

            UnityEngine.Object.Destroy( pixels );
        }


        //------------------------------------------------------------------------------

        public static void ApplicationScreenshot( RequestAdapter req, string path )
        {
            UniumComponent.Singleton.StartCoroutine( ApplicationScreenshot( req ) );
        }

        static IEnumerator ApplicationScreenshot( RequestAdapter req )
        {
            var filename = "screenshot.png";

            if( Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer )
            {
                filename = Application.persistentDataPath + '/' + filename;
            }

#if UNITY_5
            Application.CaptureScreenshot( filename );
#else
            ScreenCapture.CaptureScreenshot( filename );
#endif

            yield return new WaitForSeconds( 1.0f );

            req.Redirect( "/file/persistent/screenshot.png?" + Util.RandomString( 6 ) );
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
                IPAddress   = Util.DetectPublicIPAddress(),
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
