using System;
using System.Threading;
using NailsFramework.UnitOfWork.Session;
using NUnit.Framework;

namespace NailsFramework.Tests.UnitOfWork.Session
{
    [TestFixture]
    public class CallExecutionContextTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            context = new CallExecutionContext();
        }

        #endregion

        private CallExecutionContext context;

        [Test]
        public void DifferentContextsByCall()
        {
            var value1 = 0;
            var value2 = 0;
            var thread1 = new Thread(() =>
                                         {
                                             Assert.Throws<NullReferenceException>(() => context.GetObject<int>("Test"));
                                             context.SetObject("Test", 123);
                                             value1 = context.GetObject<int>("Test");
                                         });
            var thread2 = new Thread(() =>
                                         {
                                             Assert.Throws<NullReferenceException>(() => context.GetObject<int>("Test"));
                                             context.SetObject("Test", 456);
                                             value2 = context.GetObject<int>("Test");
                                         });
            thread1.Start();
            thread2.Start();

            Thread.Sleep(3000);

            Assert.AreEqual(123, value1);
            Assert.AreEqual(456, value2);
        }
    }
}