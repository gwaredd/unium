// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using gw.unium;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using gw.gql;
using System.Collections;


//
// UniumTestEndPoint uses the EventSystem to simulate mouse events to UI objects
//

[RequireComponent(typeof(UniumComponent))]
public class UniumSimulate : MonoBehaviour
{
    #if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )


    //////////////////////////////////////////////////////////////////////////

    public string click( int x, int y )
    {
        //Debug.Log( string.Format( "SimulateClick at {0},{1}", x, y ) );

        // raycast into scene to find game objects

        var pointerData = new PointerEventData( EventSystem.current );
        pointerData.position = new Vector2( x, y );

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pointerData, results );


        // execute click

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


    //------------------------------------------------------------------------------

    public string drag( int fx, int fy, int tx, int ty, float time )
    {
        var pointerData = new PointerEventData( EventSystem.current );
        pointerData.position = new Vector2( fx, fy );

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pointerData, results );

        foreach( var res in results )
        {
            if( ExecuteEvents.Execute( res.gameObject, pointerData, ExecuteEvents.beginDragHandler ) )
            {
                StartCoroutine( DoDrag( res.gameObject, pointerData, tx, ty, time ) );
                return UniumUtils.Path( res.gameObject );
            }
        }

        return null;
    }

    public string dragNormalised( float nfx, float nfy, float ntx, float nty, float time )
    {
        int fx, fy, tx, ty;
        Denormalise( nfx, nfy, out fx, out fy );
        Denormalise( ntx, nty, out tx, out ty );
        return drag( fx, fy, tx, ty, time );
    }

    private IEnumerator DoDrag( GameObject go, PointerEventData pointerData, int tx, int ty, float time )
    {
        yield return new WaitForEndOfFrame();

        var from = new Vector3( pointerData.position.x, pointerData.position.y );
        var to   = new Vector3( tx, ty );
        var t    = 0.0f;

        while( t < time )
        {
            t += Time.deltaTime;

            pointerData.position = Vector3.Lerp( from, to, t / time );
            ExecuteEvents.Execute( go, pointerData, ExecuteEvents.dragHandler );

            yield return new WaitForEndOfFrame();
        }

        ExecuteEvents.Execute( go, pointerData, ExecuteEvents.endDragHandler );
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
