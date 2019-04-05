// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using UnityEngine;
using System.Collections;
using System.Text;

using gw.unium;
using gw.proto.http;

#if UNITY_2017_3_OR_NEWER
using UnityEngine.Networking;
#endif

// registers a the running game with an external manager application

public class UniumMinion : MonoBehaviour
{
    public string URL;

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator Start()
    {
        // check we have a url

        if( string.IsNullOrEmpty( URL ) )
        {
            yield break;
        }

        // dispatch as either restful or sockets

        if( URL.StartsWith( "ws://", System.StringComparison.InvariantCultureIgnoreCase ) )
        {
            yield return OverlordSockets();
        }
        else
        {
            yield return OverlordRestful();
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator OverlordSockets()
    {
        throw new System.NotImplementedException();
        //yield break;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator OverlordRestful()
    {
        // get about

        var route = Unium.RoutesHTTP.Find( "/about" );

        if( route == null || route.Handler == null )
        {
            Debug.LogWarning( "UniumMinion failed to find valid /about route" );
            yield break;
        }

        var req = new RequestAdapterMemory( "/about" );
        route.Dispatch( req );

        if( req.IsError )
        {
            Debug.LogWarning( "UniumMinion failed to get /about data" );
            yield break;
        }

        // post data to end point
        bool isError = false;

#if UNITY_2017_3_OR_NEWER
        var www = UnityWebRequest.Post( URL, req.Data );
        yield return www.SendWebRequest();
        isError = www.isNetworkError || www.isHttpError;
#else
        var www = new WWW( URL, Encoding.UTF8.GetBytes( req.Data ) );
        yield return www;
        isError = www.error != null;
#endif

        if( isError )
        {
            Debug.LogWarning( "UniumMinion failed to register with overlord: " + www.error );
        }
        else
        {
            Debug.Log( "UniumMinion registered with overlord OK" );
        }
    }

#endif
}
