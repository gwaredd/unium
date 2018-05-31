using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Linq;

using gw.gql;

public class Address : MonoBehaviour
{
    private void Start()
    {
        var IPText = GetComponent<Text>();
        var port = UniumComponent.Singleton != null ? ":" + UniumComponent.Singleton.Port.ToString() : "";
        IPText.text = UniumUtils.GetIPAddress().ToString() + port;
    }
}
