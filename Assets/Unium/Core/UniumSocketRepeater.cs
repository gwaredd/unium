// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;

namespace gw.unium
{
    public partial class UniumSocket
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // periodically repeats a request - this is useful for watching variables over time

        class Repeater
        {
            public string   ID          { get { return mRequest.ID; } }
            public bool     IsFinished  { get; private set; }


            //----------------------------------------------------------------------------------------------------

            Route mRoute;
            RequestAdapterSocket mRequest;

            int     mCount  = 0;
            float   mTimer  = 0.0f;

            int     mFrom   = 0;
            int     mTo     = 0;
            float   mFreq   = 1.0f;


            public Repeater( Route route, RequestAdapterSocket req )
            {
                mRoute      = route;
                mRequest    = req;

                var repeat = req.Message.repeat;

                mFrom   = repeat.skip;
                mTo     = repeat.samples != int.MaxValue ? mFrom + repeat.samples : int.MaxValue;
                mFreq   = repeat.freq;

                mRequest.Info( "repeating" );
            }


            //----------------------------------------------------------------------------------------------------

            public void Cancel()
            {
                mRequest.Info( "stopped" );
                IsFinished = true;
            }


            //----------------------------------------------------------------------------------------------------

            public void Tick()
            {
                if( IsFinished )
                {
                    return;
                }

                // update timer
                
                mTimer += Time.deltaTime;

                if( mTimer < mFreq )
                {
                    return;
                }

                mTimer = 0.0f;


                // skip start?

                if( mCount++ < mFrom )
                {
                    return;
                }


                // dispatch route

                mRoute.Dispatch( mRequest );

                if( mRequest.Rejected )
                {
                    mRequest.Info( "cancelled" );
                    IsFinished = true;
                }

                // finished?

                else if( mCount == mTo )
                {
                    IsFinished = true;
                    mRequest.Info( "finished" );
                }
            }
        }
    }
}

#endif
