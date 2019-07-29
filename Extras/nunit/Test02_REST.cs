using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Unium.Test
{
    [TestFixture]
    public class Test02_REST
    {
        [Test]
        public async Task IsTutorialLevelRunning()
        {
            using( var client = new HttpClient() )
            {
                var res = await client.GetAsync( $"{TestConfig.URL}/about" );
                Assert.IsTrue( res.IsSuccessStatusCode );

                var content = await res.Content.ReadAsStringAsync();
                dynamic about = JToken.Parse( content );

                Assert.IsNotNull( about );
                Assert.AreEqual( "gwaredd",  (string) about.Company );
                Assert.AreEqual( "unium",    (string) about.Product );
                Assert.AreEqual( "Tutorial", (string) about.Scene );
            }
        }
    }
}


