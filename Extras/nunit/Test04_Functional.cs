using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Unium.Helpers;

namespace Unium.Test
{
    [TestFixture]
    public class Test04_Functional
    {
        [Test]
        public async Task CollectPickups()
        {
            using( var u = new WebsocketHelper() )
            {
                // connect to game

                await u.Connect( TestConfig.WS );


                // check this is the unium project running

                dynamic about = await u.Get( "/about" );

                Assert.AreEqual( "gwaredd", (string) about.Company );
                Assert.AreEqual( "unium", (string) about.Product );
                Assert.AreEqual( "Tutorial", (string) about.Scene );


                // reload the tutorial scene (ensures we are in the start state)

                dynamic scene = await u.Get( "/utils/scene/Tutorial" );

                Assert.IsNotNull( scene.scene );
                Assert.AreEqual( "Tutorial", (string) scene.scene );


                // get positions of all pickups

                dynamic pickups = await u.Get( "/q/scene/Game/Pickup.transform.position" );

                Assert.IsNotNull( pickups.Count == 4 );

                // collect pickups

                await u.Bind( "/scene/Game.Tutorial.OnPickupCollected" );

                foreach( JToken pickup in pickups )
                {
                    var pos = pickup.ToString( Formatting.None );

                    await u.Get( $"/q/scene/Game/Player.Player.MoveTo({pos})" );
                    await u.WaitFor( "OnPickupCollected", 10.0f );
                }


                // check there are no more pickups left in the level

                dynamic pickupsLeft = await u.Get( "/q/scene/Game/Pickup.name" );
                Assert.IsNotNull( pickupsLeft.Count == 0 );


                // diconnect

                await u.Disconnect();
            }
        }
    }
}
