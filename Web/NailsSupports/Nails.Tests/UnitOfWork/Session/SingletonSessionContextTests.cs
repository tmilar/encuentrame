using System.Threading;
using NailsFramework.UnitOfWork.Session;
using NUnit.Framework;

namespace NailsFramework.Tests.UnitOfWork.Session
{
    [TestFixture]
    public class SingletonSessionContextTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            context = new SingletonSessionContext();
        }

        #endregion

        private SingletonSessionContext context;

        [Test]
        public void SameContextInDifferentCalls()
        {
            var value1 = 0;
            var value2 = 0;
            context.SetObject("Test", 123);
            var thread1 = new Thread(() => { value1 = context.GetObject<int>("Test"); });
            var thread2 = new Thread(() => { value2 = context.GetObject<int>("Test"); });
            thread1.Start();
            thread2.Start();

            Thread.Sleep(3000);

            Assert.AreEqual(123, value1);
            Assert.AreEqual(123, value2);
        }
    }
}