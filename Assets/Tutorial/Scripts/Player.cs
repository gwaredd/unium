using UnityEngine;

// navmesh moved to UnityEngine.AI in 2017
#if !UNITY_5
using UnityEngine.AI;
#endif


public class Player : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo( Vector3 pos )
    {
        agent.destination = pos;
    }

    void Update()
    {
        if( Input.GetMouseButtonDown( 0 ) )
        {
            RaycastHit hit;

            if( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out hit, 100 ) )
            {
                MoveTo( hit.point );
            }
        }
    }
}
