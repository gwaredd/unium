using gw.proto.utils;
using UnityEngine;
using UnityEngine.UI;

public class Address : MonoBehaviour
{
    private void Start()
    {
        var IPText = GetComponent<Text>();
        var port = UniumComponent.Singleton != null ? ":" + UniumComponent.Singleton.Port.ToString() : "";
        IPText.text = Util.DetectPublicIPAddress() + port;
    }
}
