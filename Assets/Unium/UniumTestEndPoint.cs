// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using gw.unium;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using gw.gql;


//
// UniumTestEndPoint uses the EventSystem to simulate mouse events to UI objects
//

[RequireComponent(typeof(UniumComponent))]
public class UniumTestEndPoint : MonoBehaviour
{
    #if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )


    //////////////////////////////////////////////////////////////////////////

    public string swipe()
    {
        return null;
    }

    public string click( int x, int y )
    {
        //Debug.Log( string.Format( "SimulateClick at {0},{1}", x, y ) );

        var pointerData = new PointerEventData( EventSystem.current );
        pointerData.position = new Vector2( x, y );

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pointerData, results );

        foreach( var res in results )
        {
            if( ExecuteEvents.Execute( res.gameObject, pointerData, ExecuteEvents.pointerClickHandler ) )
            {
                return UniumUtils.Path( res.gameObject );
            }
        }

        return null;
    }


    public string clickNormalised( float nx, float ny )
    {
        int x, y;
        Denormalise( nx, ny, out x, out y );
        return click( x, y );
    }



    //////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        Unium.Root.Add( "simulate", this );
    }


    //------------------------------------------------------------------------------

    private void Denormalise( float nx, float ny, out int px, out int py )
    {
        px = (int) ( nx * Screen.width );
        py = (int) ( ny * Screen.height );
    }


    #endif
}
