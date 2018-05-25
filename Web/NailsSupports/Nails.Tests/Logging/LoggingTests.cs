using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Tests.Logging.TestModel;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Logging
{
    [TestFixture]
    public class LoggingTests : BaseTest
    {
        private static void AssertLog(ILog log, string name)
        {
            var nullLog = (NullLog) log;
            Assert.AreEqual(name, nullLog.LogName);
        }

        [Test]
        public void ShouldSetLogNameAsLemmingName()
        {
            Nails.Configure()
                .IoC.Container<Unity>()
                .StaticReference(() => ClassWithStaticLogger.Log)
                .Lemming<ClassWithInstanceLogger>("Test1")
                .Lemming<ClassWithInstanceLogger>("Test2")
                .Lemming<ClassWithInstanceLogger>()
                .Initialize();

            AssertLog(ClassWithStaticLogger.Log, "NailsFramework.Tests.Logging.TestModel.ClassWithStaticLogger");
            AssertLog(Nails.ObjectFactory.GetObject<ClassWithInstanceLogger>("Test1").Log, "Test1");
            AssertLog(Nails.ObjectFactory.GetObject<ClassWithInstanceLogger>("Test2").Log, "Test2");
            AssertLog(
                Nails.ObjectFactory.GetObject<ClassWithInstanceLogger>(
                    "NailsFramework.Tests.Logging.TestModel.ClassWithInstanceLogger").Log,
                "NailsFramework.Tests.Logging.TestModel.ClassWithInstanceLogger");
        }
    }
}