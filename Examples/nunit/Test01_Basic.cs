using NUnit.Framework;

namespace Unium.Test
{
    [TestFixture]
    public class Test01_Basic
    {
        [Test]
        public void SomeTest()
        {
            var someVar = 123;
            Assert.AreEqual( 123, someVar );
        }
    }
}

