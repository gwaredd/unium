using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool OpenBrowser = true;
    public Text PickupText;
    public GameObject Reminder;

    static bool OpenOnce    = true;

    int         mNumPickups = 0;

    // exposed event for tutorial script to hook into
    public event Action<object> OnPickupCollected;

    void Start()
    {
        var index = Path.Combine( Application.streamingAssetsPath, "tutorial/index.html" );

        if( File.Exists( index ) )
        {
            Reminder.gameObject.SetActive( false );

            if( OpenBrowser && OpenOnce )
            {
                OpenOnce = false;
                System.Diagnostics.Process.Start( "http://localhost:8342/tutorial/index.html" );
            }
        }
        else
        {
            Reminder.gameObject.SetActive( true );
            Debug.LogError( "Failed to find /StreamingAssets/tutorial/index.html" );
            Debug.LogError( "Please unzip the tutorial.zip file to the StreamingAssets folder" );
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
