using NUnit.Framework;
using System.Threading.Tasks;

namespace Unium.Test
{
    [TestFixture]
    public class Test04_Functional
    {
        [Test]
        public async Task CollectPickups()
        {
            using( var u = new Helpers.WebsocketHelper() )
            {
                await u.Connect( TestConfig.WS );

                await u.Send( "/about" );
                var str = await u.Recv();

                await u.Disconnect();
            }
        }
    }
}
