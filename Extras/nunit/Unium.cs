using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unium.Helpers
{
    ////////////////////////////////////////////////////////////////////////////////

    public class About
    {
        public string Company;
        public string Product;
        public string Scene;
    }

    public class Message
    {
        public string id;
    }

    public class AboutMessage : Message
    {
        public About data;
    }


    ////////////////////////////////////////////////////////////////////////////////

    public class WebsocketHelper : IDisposable
    {
        private int             mNextID = 1;
        private ClientWebSocket mWS     = null;
        private byte[]          mBuffer = new byte[ 64 * 1024 ];

        private Dictionary<string, List<Action<string>>> mOn;
        private Dictionary<string, List<Action<string>>> mOnce;

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
            mOn     = new Dictionary<string, List<Action<string>>>();
            mOnce   = new Dictionary<string, List<Action<string>>>();
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

        private void OnMessage( string id, string data )
        {
            if( string.IsNullOrWhiteSpace( id ) )
            {
                return;
            }

            if( mOn.ContainsKey( id ) )
            {
                mOn[ id ].ForEach( h => h( data ) );
            }

            if( mOnce.ContainsKey( id ) )
            {
                mOnce[ id ].ForEach( h => h( data ) );
                mOnce.Remove( id );
            }
        }

        public void On( string id, Action<string> callback )
        {
            if( mOn.ContainsKey( id ) == false )
            {
                mOn.Add( id, new List<Action<string>>() );
            }

            mOn[ id ].Add( callback );
        }

        public void Once( string id, Action<string> callback )
        {
            if( mOnce.ContainsKey( id ) == false )
            {
                mOnce.Add( id, new List<Action<string>>() );
            }

            mOnce[ id ].Add( callback );
        }

        public async Task<string> Send( string q, string name = null )
        {
            var id    = name ?? $"m{mNextID++}";
            var query = Encoding.ASCII.GetBytes( $@"{{""id"":""{id}"",""q"":""{q}""}}" );

            await mWS.SendAsync( new ArraySegment<byte>( query ), WebSocketMessageType.Text, true, CancellationToken.None );

            return id;
        }

        public async Task<string> Recv( int timeout = 2 )
        {
            var buffer   = new ArraySegment<byte>( mBuffer, 0, mBuffer.Length );
            var response = null as WebSocketReceiveResult;

            using( var ct = new CancellationTokenSource( timeout * 1000 ) )
            {
                response = await mWS.ReceiveAsync( buffer, ct.Token );
            }

            Assert.AreEqual( WebSocketMessageType.Text, response.MessageType );

            var message = Encoding.UTF8.GetString( mBuffer );
            var header  = JsonConvert.DeserializeObject<Message>( message );

            OnMessage( header.id, message );

            return message;
        }

        private async Task<string> GetInternal( string uri )
        {
            throw new NotImplementedException();
        }

        public async Task<string> Get( string uri, int timeout = 2 )
        {
            var task = GetInternal( uri );

            if( await Task.WhenAny( task, Task.Delay( timeout * 1000 ) ) != task )
            {
                Assert.Fail( "Timeout" );
            }

            return task.Result;
        }
    }
}
