using gw.proto.utils;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string   ID             = null;
    public float    RotateSpeed    = 0.5f;
    public float    BobSpeed       = 0.01f;
    public float    BobHeight      = 0.2f;

    float mStartY   = 0.0f;
    float mT        = 0.0f;

    void Start()
    {
        ID = Util.RandomString( 6 );
        SendMessageUpwards( "PickupCreated" );

        mStartY = transform.position.y;
    }

    void Update()
    {
        mT += Time.deltaTime;
        var y = mStartY + Mathf.Sin( mT * BobSpeed * 360.0f ) * BobHeight;

        transform.Rotate( Vector3.up, Time.deltaTime * 360.0f * RotateSpeed );
        transform.position = new Vector3( transform.position.x, y, transform.position.z );
    }

    void OnTriggerEnter( Collider other )
    {
        SendMessageUpwards( "PickupCollected" );
        Destroy( gameObject );
    }
}
