using UnityEngine;

public class TestEvents : MonoBehaviour
{
    public delegate void MyEvent();
    public event MyEvent myEvent;

    void Start()
    {
        myEvent += OnEvent;
    }

    void OnEvent()
    {
        Debug.Log( "MyEvent fired" );
    }

    public void TriggerEvent()
    {
        myEvent();
    }
}
