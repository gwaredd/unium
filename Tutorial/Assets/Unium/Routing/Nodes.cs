// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Stats
    {
        public static Stats Singleton = new Stats();

        public float    FPS             { get { return 1.0f / Time.smoothDeltaTime; } }
        public float    RunningTime     { get { return Time.realtimeSinceStartup; } }
        public float    LevelTime       { get { return Time.timeSinceLevelLoad; } }
        public string   Scene           { get { return SceneManager.GetActiveScene().name; } }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Events
    {
        public static Events Singleton = new Events();

        public event Action<object> debug;
        public event Action<object> sceneLoaded;


        //----------------------------------------------------------------------------------------------------

        public void BindEvents()
        {
            Application.logMessageReceivedThreaded += OnLogMessage;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void UnbindEvents()
        {
            Application.logMessageReceivedThreaded -= OnLogMessage;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        //----------------------------------------------------------------------------------------------------

        private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            if( sceneLoaded == null )
            {
                return;
            }

            sceneLoaded( new {
                name = scene.name
            });
        }

        private void OnLogMessage( string condition, string stackTrace, LogType type )
        {
            if( debug == null )
            {
                return;
            }

            debug( new {
                message = condition,
                type    = type.ToString(),
                //stack   = stackTrace
            });
        }
    }
}

#endif
