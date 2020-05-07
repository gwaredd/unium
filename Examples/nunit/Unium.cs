using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unium.Helpers
{
    public class WebsocketHelper : IDisposable
    {
        private int             mNextID = 1;
        private ClientWebSocket mWS     = null;

        private Dictionary<string, List<Action<JToken>>> mOn;
        private Dictionary<string, List<Action<JToken>>> mOnce;


        public async Task Connect( string uri )
        {
            mWS = new ClientWebSocket();

            using( var ct = new CancellationTokenSource( 2000 ) )
            {
                var taskConnect = mWS.ConnectAsync( new Uri( uri ), ct.Token );

                Assert.True(
                    ( mWS.State == WebSocketState.None ) ||
                    ( mWS.State == WebSocketState.Connecting ) ||
                    ( mWS.State == WebSocketState.Open )
                );

                await taskConnect;
            }

            Assert.AreEqual( WebSocketState.Open, mWS.State );

            mNextID = 1;
            mOn     = new Dictionary<string, List<Action<JToken>>>();
            mOnce   = new Dictionary<string, List<Action<JToken>>>();
        }


        public async Task Disconnect()
        {
            await mWS.CloseAsync( WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None );
            mOn   = null;
            mOnce = null;
        }


        public void Dispose()
        {
            if( mWS != null )
            {
                mWS.Dispose();
            }
        }


        private void OnMessage( string id, JToken msg )
        {
            if( string.IsNullOrWhiteSpace( id ) )
            {
                return;
            }

            if( mOn.ContainsKey( id ) )
            {
                mOn[ id ].ForEach( h => h( msg ) );
            }

            if( mOnce.ContainsKey( id ) )
            {
                mOnce[ id ].ForEach( h => h( msg ) );
                mOnce.Remove( id );
            }
        }


        public void On( string id, Action<JToken> callback )
        {
            if( mOn.ContainsKey( id ) == false )
            {
                mOn.Add( id, new List<Action<JToken>>() );
            }

            mOn[ id ].Add( callback );
        }


        public void Once( string id, Action<JToken> callback )
        {
            if( mOnce.ContainsKey( id ) == false )
            {
                mOnce.Add( id, new List<Action<JToken>>() );
            }

            mOnce[ id ].Add( callback );
        }


        public string Send( string q, string name = null )
        {
            var id    = name ?? $"m{mNextID++}";
            var query = Encoding.ASCII.GetBytes( $@"{{""id"":""{id}"",""q"":""{q}""}}" );

            mWS.SendAsync( new ArraySegment<byte>( query ), WebSocketMessageType.Text, true, CancellationToken.None );

            return id;
        }


        private async Task<JToken> WaitForTask( string id )
        {
            while( true )
            {
                var buffer  = new byte[ 2048 ];
                var segment = new ArraySegment<byte>( buffer, 0, buffer.Length );

                var recv = await mWS.ReceiveAsync( segment, CancellationToken.None );
                Assert.AreEqual( WebSocketMessageType.Text, recv.MessageType );

                var data = Encoding.UTF8.GetString( buffer );
                var msg  = JToken.Parse( data );

                var mid = msg.Value<string>( "id" );

                OnMessage( mid, data );

                if( id == null || mid == id )
                {
                    return msg;
                }
            }
        }

        public async Task<JToken> WaitFor( string id, float timeout = 2.0f )
        {
            var task = WaitForTask( id );

            if( await Task.WhenAny( task, Task.Delay( (int) ( timeout * 1000.0f ) ) ) != task )
            {
                Assert.Fail( "Timeout" );
            }

            return task.Result;
        }

        public async Task<JToken> Get( string uri, float timeout = 2.0f )
        {
            var id  = Send( uri );
            var res = await WaitFor( id, timeout );
            return res.Value<JToken>( "data" );
        }


        public async Task Bind( string uri, string name = null )
        {
            var parts = uri.Split( new char[] { '.' } );
            var eventName = parts[ parts.Length - 1 ];

            string id = name ?? eventName;

            Send( $"/bind{uri}", id );

            dynamic res = await WaitFor( id );

            Assert.IsNull( (string) res.error );
            Assert.AreEqual( id, (string) res.id );
            Assert.AreEqual( "bound", (string) res.info );
        }
    }
}
