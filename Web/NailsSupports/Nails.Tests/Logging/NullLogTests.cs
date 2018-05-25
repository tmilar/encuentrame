using NailsFramework.Logging;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Logging
{
    [TestFixture]
    public class NullLogTests : BaseTest
    {
        [Test]
        public void ShouldDoNothing()
        {
            new NullObjectTester().Test<ILog>(new NullLog());
            var logger = new NullLogger();
            logger.AddCustomConfiguration(Nails.Configure());
            logger.Initialize();
        }
    }
}