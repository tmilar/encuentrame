using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC
{
    [TestFixture]
    public class NullObjectFactoryTests
    {
        [Test]
        public void ShouldDoNothing()
        {
            var objectFactory = new NullObjectFactory();
            new NullObjectTester().Test<IObjectFactory>(objectFactory);
            Assert.IsNull(objectFactory.GetObject<NullObjectFactory>());
            Assert.IsNull(objectFactory.GetObject<NullObjectFactory>("bla"));
            Assert.AreEqual(0, objectFactory.GetObjects<NullObjectFactory>().Count());
            Assert.IsNull(objectFactory.GetGenericObject<string>("bla"));
            Assert.IsNull(objectFactory.GetGenericObject<string, string>("bla"));
            Assert.IsNull(objectFactory.GetGenericObject<string, string, string>("bla"));
            Assert.IsNull(objectFactory.GetGenericObject<string, string, string, string>("bla"));
            Assert.IsNull(objectFactory.GetGenericObject<string, string, string, string, string>("bla"));
        }
    }
}