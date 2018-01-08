using UnityEngine;

public class Animate : MonoBehaviour
{
    public float Height = 5.0f;
    public float Speed  = 1.0f;

    float mT = 0.0f;

    void Update()
    {
        mT += Time.deltaTime;

        var y = Height * Mathf.Sin( mT * Mathf.PI * 2 * Speed );

        transform.position = new Vector3( transform.position.x, y, transform.position.z );
    }
}
