// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

using gw.gql;
using gw.proto.utils;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // unium configuration

    static class Unium
    {
        public delegate void EventDelegate( object data );

        public static readonly Version  Version         = new Version( 1, 0, 1 );
        public static Router            RoutesHTTP      = new Router();
        public static Router            RoutesSocket    = new Router();
        public static Root              Root            = new Root();

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

        //----------------------------------------------------------------------------------------------------

        static Unium()
        {
            SetupTypeHandlers();
            SetupRouter();
            SetupGQL();
        }


        //----------------------------------------------------------------------------------------------------
        // serialisers for value types

        static void SetupTypeHandlers()
        {
            JsonReflector.Add( typeof( GameObject ),    new SerialiseGameObject() );
            JsonReflector.Add( typeof( Transform ),     new SerialiseTransform() );
        }


        //----------------------------------------------------------------------------------------------------

        static void SetupRouter()
        {
            //
            // HTTP routes
            //

            // game thread

            RoutesHTTP.Add( "/q",                   HandlerGQL.Query );
            RoutesHTTP.Add( "/about",               HandlerUtils.HandlerAbout );

            RoutesHTTP.Add( "/utils/debug",         HandlerUtils.DebugOutput );
            RoutesHTTP.Add( "/utils/screenshot",    HandlerUtils.Screenshot );
            RoutesHTTP.Add( "/utils/appscreenshot", HandlerUtils.ApplicationScreenshot );
            RoutesHTTP.Add( "/utils/scene",         HandlerUtils.HandlerScene );
            RoutesHTTP.Add( "/utils",               HandlerUtils.NotFound );

            RoutesHTTP.Add( "/", ( RequestAdapter req, string path ) => req.Redirect( "index.html" ) ).ExactMatch = true;

            // android requires the WWW interface, so dispatch file requests on the game thread
            // otherwise we can dispatch immediately on a worker thread

            if( Application.platform == RuntimePlatform.Android )
            {
                RoutesHTTP.Add( "/file", HandlerFile.Serve );
            }
            else
            {
                RoutesHTTP.AddImmediate( "/file", HandlerFile.Serve );
            }

            // default route handler if all others fail

            RoutesHTTP.Otherwise = new Route()
            {
                Handler              = HandlerFile.Serve,
                DispatchOnGameThread = Application.platform == RuntimePlatform.Android
            };


            //
            // Socket routes
            //

            RoutesSocket.Add( "/q",             HandlerGQL.Query ).CacheContext = HandlerGQL.CacheContext;
            RoutesSocket.Add( "/about",         HandlerUtils.HandlerAbout );
            RoutesSocket.Add( "/utils/scene",   HandlerUtils.HandlerScene );

            RoutesSocket.Add( "/socket",        HandlerSocketCommand.Execute );
            RoutesSocket.Add( "/bind",          HandlerSocketBind.HandleBind );
        }



        //----------------------------------------------------------------------------------------------------

        static void SetupGQL()
        {
            // node interpreters for types

            Interpreters.Add( typeof( GameObject ),     new InterpreterGameObject() );
            Interpreters.Add( typeof( GameObject[] ),   new InterpreterGameObjectArray() );
            Interpreters.Add( typeof( Root ),           new InterpreterSearchRoot() );

            // /q/scene queries root-level game objects from all loaded scenes, and the "DontDestroyOnScene" pseudo-scene if running

            Func<object> scene = () =>
                Enumerable
                    .Range( 0, SceneManager.sceneCount )
                    .SelectMany( i => SceneManager.GetSceneAt(i).GetRootGameObjects() )
                    .Concat( UniumComponent.Singleton ? UniumComponent.Singleton.gameObject.scene.GetRootGameObjects() : Enumerable.Empty<GameObject>() )  
                    .ToArray();

            Root.Add( "scene",  scene );
            Root.Add( "stats",  Stats.Singleton );
            Root.Add( "events", Events.Singleton );
        }
#endif
    }
}
