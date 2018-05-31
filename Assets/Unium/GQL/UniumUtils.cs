// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace gw.gql
{
    public static class UniumUtils
    {
        public static IPAddress GetIPAddress()
        {
            if( System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false )
            {
                return IPAddress.Any;
            }

            var host = Dns.GetHostEntry( Dns.GetHostName() );
            return host.AddressList.FirstOrDefault( ip => ip.AddressFamily == AddressFamily.InterNetwork );
        }
        
        public static string Path( GameObject go )
        {
            if( go == null )
            {
                return null;
            }

            var path = new List<string>();
            var xf = go.transform;

            while( xf != null )
            {
                path.Add( xf.name );
                xf = xf.parent;
            }

            path.Reverse();

            return "/" + string.Join( "/", path.ToArray() );
        }
    }
}

#endif
