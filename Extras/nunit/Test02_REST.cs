using Newtonsoft.Json;
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
                var about = JsonConvert.DeserializeObject<Helpers.About>( content );

                Assert.AreEqual( "gwaredd",  about.Company );
                Assert.AreEqual( "unium",    about.Product );
                Assert.AreEqual( "Tutorial", about.Scene );
            }
        }
    }
}


