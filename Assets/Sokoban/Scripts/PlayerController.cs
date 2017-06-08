using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    Sokoban     sokoban;
    Animator    animator;
    bool        moving = false;

    public AudioClip footstep;
    public AudioClip dink;

    Vector3 startPos;
    Vector3 targetPos;
    float   moveTimer = 0.0f;

    Vector3 mouseStart = Vector3.zero;

    public float Speed = 1.0f;

    void Start()
    {
        sokoban = GameObject.FindGameObjectWithTag( "Game" ).GetComponent<Sokoban>();

        animator = transform.GetChild( 0 ).GetComponent<Animator>();
    }

    void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            mouseStart = Input.mousePosition;
        }

        if( moving )
        {
            UpdateMoving();
            return;
        }

        // key input

        if( Input.GetKeyDown( KeyCode.LeftArrow ) )
        {
            DoMove( Vector3.left, 270 );
        }
        else if( Input.GetKeyDown( KeyCode.RightArrow ) )
        {
            DoMove( Vector3.right, 90 );
        }
        else if( Input.GetKeyDown( KeyCode.UpArrow ) )
        {
            DoMove( Vector3.forward, 0 );
        }
        else if( Input.GetKeyDown( KeyCode.DownArrow ) )
        {
            DoMove( Vector3.back, 180 );
        }

        // swipe input

        if( Input.GetMouseButtonUp(0) )
        {
            var delta = Input.mousePosition - mouseStart;

            if( delta.magnitude > 20 )
            {
                if( Mathf.Abs( delta.x ) > Mathf.Abs( delta.y ) )
                {
                    if( delta.x > 0.0f )
                    {
                        DoMove( Vector3.right, 90 );
                    }
                    else
                    {
                        DoMove( Vector3.left, 270 );
                    }
                }
                else
                {
                    if( delta.y > 0.0f )
                    {
                        DoMove( Vector3.forward, 0 );
                    }
                    else
                    {
                        DoMove( Vector3.back, 180 );
                    }
                }
            }
        }
    }

    public void ApplyMove( KeyCode key )
    {
        switch( key )
        {
            case KeyCode.LeftArrow:
                DoMove( Vector3.left, 270 );
                break;

            case KeyCode.RightArrow:
                DoMove( Vector3.right, 90 );
                break;

            case KeyCode.UpArrow:
                DoMove( Vector3.forward, 0 );
                break;

            case KeyCode.DownArrow:
                DoMove( Vector3.back, 180 );
                break;

        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    void UpdateMoving()
    {
        moveTimer = Mathf.Clamp01( moveTimer + Time.deltaTime * Speed );

        transform.position = Vector3.Lerp( startPos, targetPos, moveTimer ); ;

        if( moveTimer >= 1.0f )
        {
            moving = false;
            animator.SetBool( "Walking", false );
        }
    }

    bool PushBox( GameObject box, Vector3 dir )
    {
        var crate = box.GetComponent<CrateController>();

        if( crate != null )
        {
            return crate.DoPush( dir );
        }

        return false;
    }

    bool CanMove( Vector3 dir )
    {
        RaycastHit hit;

        if( Physics.Raycast( transform.position, dir, out hit, 1.0f ) )
        {
            if( hit.transform.gameObject.tag == "Wall" )
            {
                return false;
            }
            else if( hit.transform.gameObject.tag == "Box" )
            {
                return PushBox( hit.transform.gameObject, dir );
            }
        }

        return true;
    }

    void DoMove( Vector3 dir, float angle )
    {
        if( CanMove( dir ) )
        {
            startPos = transform.position;
            targetPos = transform.position + dir;
            moving = true;
            moveTimer = 0.0f;
            animator.SetBool( "Walking", true );

            //transform.position += dir;
            transform.rotation = Quaternion.Euler( 0, angle, 0 );


            AudioSource audio = GetComponent<AudioSource>();
            audio.volume = Sokoban.Volume;
            audio.PlayOneShot( footstep );

            sokoban.AddMove();
        }
        else
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot( dink );
        }
    }
}

