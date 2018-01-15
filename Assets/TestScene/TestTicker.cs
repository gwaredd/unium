using System;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestTicker : MonoBehaviour
{
    public int TickerID = 0;
    public event Action<object> TickEvent;

    float mTimer = 0.0f;

    void Update()
    {
        mTimer += Time.deltaTime;

        if( mTimer < 1.0f )
        {
            return;
        }

        mTimer = 0.0f;

        if( TickEvent != null )
        {
            TickEvent( new { id = TickerID, levelTime = Time.timeSinceLevelLoad } );
        }
    }
}
