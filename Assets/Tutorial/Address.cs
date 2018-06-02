using UnityEngine;
using UnityEngine.UI;

using gw.gql;
using gw.proto.utils;

public class Address : MonoBehaviour
{
    private void Start()
    {
        var IPText = GetComponent<Text>();
        var port = UniumComponent.Singleton != null ? ":" + UniumComponent.Singleton.Port.ToString() : "";
        IPText.text = Util.GetIPAddress().ToString() + port;
    }
}
