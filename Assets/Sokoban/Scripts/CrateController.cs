using UnityEngine;
using System.Collections;

public class CrateController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    bool CanMove( Vector3 dir )
    {
        RaycastHit hit;

        if( Physics.Raycast( transform.position, dir, out hit, 1.0f ) )
        {
            if( hit.transform.gameObject.tag != "Goal" )
            {
                return false;
            }
        }

        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool DoPush( Vector3 dir )
    {
        if( !CanMove(dir) )
        {
            return false;
        }

        transform.position += dir;
        return true;
    }
}


