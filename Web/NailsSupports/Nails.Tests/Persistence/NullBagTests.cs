using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    [TestFixture]
    public class NullBagTests
    {
        public class MockSubClass : MockModel
        {
        }

        [Test]
        public void ShouldDoNothing()
        {
            var bag = new NullBag<MockModel>();
            new NullObjectTester().Test<IBag<MockModel>>(bag);
        }
    }
}