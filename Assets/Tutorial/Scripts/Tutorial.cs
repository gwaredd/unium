using System;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool OpenBrowser = true;
    public Text PickupText;

    static bool OpenOnce    = true;

    int         mNumPickups = 0;

    // exposed event for tutorial script to hook into
    public event Action<object> OnPickupCollected;

    void Start()
    {
        if( OpenBrowser && OpenOnce )
        {
            OpenOnce = false;
            System.Diagnostics.Process.Start( "http://localhost:8342/tutorial/index.html" );
        }
    }

    void PickupCreated()
    {
        mNumPickups++;
        PickupText.text = mNumPickups.ToString();
    }

    void PickupCollected()
    {
        mNumPickups--;
        PickupText.text = mNumPickups.ToString();

        GetComponent<AudioSource>().Play();

        Debug.Log( "Pickup collected" );

        if( OnPickupCollected != null )
        {
            OnPickupCollected( "Pickup Collected" );
        }
    }

    public void SayHello()
    {
        Debug.Log( "Hello from Unium" );
    }
}
