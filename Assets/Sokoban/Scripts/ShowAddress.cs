using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class ShowAddress : MonoBehaviour
{
    void Start()
    {
        var text = GetComponent<Text>();

        text.text = Network.player.ipAddress;

//        // find first IP4 external address
//
//        foreach( var ip in Dns.GetHostEntry( Dns.GetHostName() ).AddressList )
//        {
//            if( ip.AddressFamily == AddressFamily.InterNetwork  )
//            {
//                text.text = ip.ToString();
//                break;
//            }
//        }
    }
}
