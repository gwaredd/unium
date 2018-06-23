using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject   Target;
    public float        Distance    = 10.0f;
    public float        Height      = 10.0f;

    void Update()
    {
        var pos = Target.transform.position - Target.transform.forward * Distance;
        pos.y = Target.transform.position.y + Height;

        Camera.main.transform.position = pos;
        Camera.main.transform.LookAt( Target.transform, Vector3.up );

    }
}
