using UnityEngine;
using UnityEngine.UI;

public class Address : MonoBehaviour
{
    private void Start()
    {
        var IPText = GetComponent<Text>();

        if( IPText != null )
        {
            var port = UniumComponent.Singleton != null ? ":" + UniumComponent.Singleton.Port.ToString() : "";
            IPText.text = Network.player.ipAddress.ToString() + port;
        }
    }
}
