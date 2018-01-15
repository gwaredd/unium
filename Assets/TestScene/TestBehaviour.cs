using UnityEngine;

////////////////////////////////////////////////////////////////////////////////////////////////////
// class for testing GQL functionality

public class TestBehaviour : MonoBehaviour
{
    public int      SomeID      = 1;
    public float    RandomValue = 0.0f;

    void Start()
    {
        RandomValue = Random.value;
    }

    public int CallThisFunction()
    {
        Debug.Log( string.Format( "CallThisFunction() called on {0}:{1}", gameObject.name, SomeID ) );
        return SomeID;
    }

    public int CallThisFunctionWithParams( int a, int b )
    {
        return a + b;
    }
}
