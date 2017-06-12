using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool OpenBrowser = true;
    public Text PickupText;

    int NumPickups = 0;

	void Start()
    {
        if( OpenBrowser )
        {
            System.Diagnostics.Process.Start( "http://localhost:8342/tutorial/index.html" );
        }
    }

    void PickupCreated()
    {
        NumPickups++;
        PickupText.text = NumPickups.ToString();
    }

    void PickupCollected()
    {
        NumPickups--;
        PickupText.text = NumPickups.ToString();

        GetComponent<AudioSource>().Play();

        Debug.Log( "Pickup collected" );
    }
}

