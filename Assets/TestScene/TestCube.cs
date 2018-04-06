using UnityEngine;

public class TestCube : MonoBehaviour
{
    public float Speed  = 1.0f;

    void Update()
    {
        transform.Rotate( Vector3.up, 360.0f * Time.deltaTime * Speed );
    }

    private void OnMouseDown()
    {
        Debug.Log( "Cube clicked" );
    }
}
