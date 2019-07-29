using NUnit.Framework;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Unium.Test
{
    [TestFixture]
    public class Test03_WebSockets
    {
        [Test]
        public async Task IsTutorialLevelRunning()
        {
            // using ClientWebSocket - we simplify this with a helper class in the next example

            using( var ws = new ClientWebSocket() )
            {
                using( var ct = new CancellationTokenSource( 2000 ) )
                {
                    var taskConnect = ws.ConnectAsync( new Uri( TestConfig.WS ), ct.Token );

                    Assert.True(
                        ( ws.State == WebSocketState.None ) ||
                        ( ws.State == WebSocketState.Connecting ) ||
                        ( ws.State == WebSocketState.Open )
                    );

                    await taskConnect;
                }

                Assert.AreEqual( WebSocketState.Open, ws.State );

                // send /about
                var query = Encoding.ASCII.GetBytes( @"{""q"":""/about""}" );
                await ws.SendAsync( new ArraySegment<byte>( query ), WebSocketMessageType.Text, true, CancellationToken.None );

                // receive response
                var buffer  = new byte[ 2048 ];
                var segment = new ArraySegment<byte>( buffer, 0, buffer.Length );
                var recv    = null as WebSocketReceiveResult;

                using( var ct = new CancellationTokenSource( 2000 ) )
                {
                    recv = await ws.ReceiveAsync( segment, ct.Token );
                }

                Assert.AreEqual( WebSocketMessageType.Text, recv.MessageType );

                var response  = Encoding.UTF8.GetString( buffer );
                dynamic about = JToken.Parse( response );

                Assert.IsNotNull( about.data );
                Assert.AreEqual( "gwaredd",  (string) about.data.Company );
                Assert.AreEqual( "unium",    (string) about.data.Product );
                Assert.AreEqual( "Tutorial", (string) about.data.Scene );

                await ws.CloseAsync( WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None );
            }
        }
    }
}
