﻿using UnityEngine;
using gw.unium;
using System;

////////////////////////////////////////////////////////////////////////////////////////////////////

public class Test : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------

    void Start()
    {
        // check we can add routes dynamically
        Unium.RoutesHTTP.AddImmediate( "/test", ( RequestAdapter req, string path ) => req.Respond( @"{""test"":""ok""}" ) );
    }

    //----------------------------------------------------------------------------------------------------
    // enable a stream of debug output for debug events test

    public void RandomDebugMessage()
    {
        var type = LogType.Log;

        // 20% chance of warning or error

        if( UnityEngine.Random.value > 0.8f )
        {
            type = UnityEngine.Random.value < 0.6f ? LogType.Warning : LogType.Error;
        }

        Debug.unityLogger.Log( type, string.Format( "Level time is {0}", Time.timeSinceLevelLoad ) );
    }


    //----------------------------------------------------------------------------------------------------
    // custom event

    public int FrameCounter = 0;

    public event Action<object> TickEvent;

    float mTimer = 0.0f;

    void Update()
    {
        FrameCounter++;

        mTimer += Time.deltaTime;

        if( mTimer < 1.0f )
        {
            return;
        }

        mTimer = 0.0f;

        if( TickEvent != null )
        {
            TickEvent( new { levelTime = Time.timeSinceLevelLoad } );
        }
    }
}
