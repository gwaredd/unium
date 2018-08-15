// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

//#define GW_LOGGING

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Specialized;

namespace gw.proto.utils
{
    public static class Util
    {
        public static Random Rnd = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string RandomString( int length = 8 )
        {
            return new string( Enumerable.Repeat( chars, length ).Select( s => s[ Rnd.Next( s.Length ) ] ).ToArray() );
        }

        [Conditional("GW_LOGGING")]
        public static void Print( string msg, params object[] args )
        {
            UnityEngine.Debug.Log( string.Format( "[{0:HH:mm:ss.ffff}] {1}", DateTime.Now, string.Format( msg, args ) ) );
        }

        [Conditional( "GW_LOGGING" )]
        public static void Warn( string msg, params object[] args )
        {
            UnityEngine.Debug.LogWarning( string.Format( "[{0:HH:mm:ss.ffff}] {1}", DateTime.Now, string.Format( msg, args ) ) );
        }

        [Conditional( "GW_LOGGING" )]
        public static void Error( string msg, params object[] args )
        {
            UnityEngine.Debug.LogError( string.Format( "[{0:HH:mm:ss.ffff}] {1}", DateTime.Now, string.Format( msg, args ) ) );
        }

        public static string DetectPublicIPAddress()
        {
            if( NetworkInterface.GetIsNetworkAvailable() == false )
            {
                return IPAddress.Any.ToString(); // 0.0.0.0
            }

            return Dns.GetHostEntry( Dns.GetHostName() )
                .AddressList
                .Where( addr => addr.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback( addr ) )
                .LastOrDefault() // seems to be the convention :o
                .ToString()
            ;
        }

        public static NameValueCollection ParseQueryString( string query )
        {
            NameValueCollection bag = new NameValueCollection();

            char[] parameterDelimiters = new char[] { '?', '&' };
            string[] parameters = query.Split( parameterDelimiters, System.StringSplitOptions.RemoveEmptyEntries );

            char[] keyValueDelimiters = new char[] { '=' };
            for ( int i = 0; i < parameters.Length; ++i )
            {
                string[] keyValue = parameters[ i ].Split ( keyValueDelimiters, System.StringSplitOptions.None );

                if ( keyValue.Length >= 2 )
                {
                    bag.Add( UnityEngine.WWW.UnEscapeURL( keyValue[ 0 ] ), UnityEngine.WWW.UnEscapeURL( keyValue[ 1 ] ) );
                }
                else if (keyValue.Length == 1)
                {
                    bag.Add( UnityEngine.WWW.UnEscapeURL( keyValue[ 0 ] ), "");
                }
            }
            return bag;
        }
    }
}
