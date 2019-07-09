#pragma warning disable 0067

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int, int>
{
}

public class TestEvents : MonoBehaviour
{
    public delegate void MyEvent();
    public event MyEvent myEvent;

    public delegate void MyEventParams1( int a );
    public event MyEventParams1 myEventParams1;

    public delegate void MyEventParams2( int a, string b );
    public event MyEventParams2 myEventParams2;

    public delegate string MyEventReturn();
    public event MyEventReturn myEventReturn;

    public UnityEvent myUnityEvent;
    public MyIntEvent myUnityIntEvent;

    void Start()
    {
        // c# events

        myEvent += OnEvent;
        myEventParams1 += OnEventParams1;
        myEventParams1 += OnEventParams1v2;
        myEventParams2 += OnEventParams2;
        myEventReturn += OnEventReturn;
        myEventReturn += OnEventReturn;
        myEventReturn += OnEventReturnV2;

        // unity events

        if( myUnityEvent == null )
        {
            myUnityEvent = new UnityEvent();
        }

        myUnityEvent.AddListener( OnUnityEvent );

        if( myUnityIntEvent == null )
        {
            myUnityIntEvent = new MyIntEvent();
        }

        myUnityIntEvent.AddListener( OnUnityIntEvent );
    }

    void OnEvent()
    {
        Debug.Log( "MyEvent fired" );
    }

    public void TriggerEvent()
    {
        myEvent();
    }

    void OnEventParams1( int a )
    {
        Debug.Log( "OnEventParams1 " + a.ToString() );
    }

    void OnEventParams1v2( int a )
    {
        Debug.Log( "OnEventParams1v2 " + a.ToString() );
    }

    void OnEventParams2( int a, string b )
    {
        Debug.Log( "OnEventParams2 " + a.ToString() + ", " + b );
    }

    string OnEventReturn()
    {
        Debug.Log( "OnEventReturn" );
        return "fish";
    }

    string OnEventReturnV2()
    {
        Debug.Log( "OnEventReturnV2" );
        return "haddock";
    }

    void OnUnityEvent()
    {
        Debug.Log( "OnUnityEvent" );
    }

    public void TriggerUnityEvent()
    {
        myUnityEvent.Invoke();
    }

    void OnUnityIntEvent( int a, int b )
    {
        Debug.Log( "UnityIntEvent " + a.ToString() + ", " + b.ToString() );
    }
}
