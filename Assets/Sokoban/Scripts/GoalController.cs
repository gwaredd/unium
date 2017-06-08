using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour
{
    public Color colourOn;
    public Color colourOff;

    Material material = null;
    Sokoban sokoban;

    void Start()
    {
        var child = transform.GetChild( 0 );
        material = child.gameObject.GetComponent<Renderer>().material;

        material.SetColor( "_Color", colourOff );

        var game = GameObject.FindGameObjectWithTag( "Game" );

        if( game )
        {
            sokoban = game.GetComponent<Sokoban>();
        }
    }

    void OnTriggerEnter( Collider other )
    {
        if( other.tag == "Box" && material != null )
        {
            material.SetColor( "_Color", colourOn );
            sokoban.GoalActive();
        }
    }

    void OnTriggerExit( Collider other )
    {
        if(other.tag == "Box")
        {
            sokoban.GoalInactive();
            material.SetColor( "_Color", colourOff );
        }
    }
}

