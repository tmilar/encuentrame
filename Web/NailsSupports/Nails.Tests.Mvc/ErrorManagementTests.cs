using System;
using NailsFramework.Tests.Support;
using NailsFramework.UserInterface.TestingSupport;
using NUnit.Framework;

namespace NailsFramework.Mvc.Tests
{
    [TestFixture]
    public class ErrorManagementTests : BaseTest
    {
        private ErrorTestController controller;

        [SetUp]
        public void SetUp()
        {
            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .InspectAssemblyOf<ErrorTestController>()
                .UserInterface.Platform(new NailsFramework.UserInterface.Mvc
                                            {
                                                ConfigureContext = false
                                            });

            Nails.Initialize();
            controller = TestControllers.Get<ErrorTestController>();
        }

        private void Test<TException>(Action<ErrorTestController> action) where TException : Exception
        {
            controller.ForceOnException(Assert.Throws<TException>(() => action(controller)));
            var exception = controller.ViewData["Exception"];
            Assert.AreEqual(typeof(TException), exception.GetType());
        }

        [Test]
        public void ExceptionHandled()
        {
            Test<Exception>(x => x.ExceptionHandled());
        }

        [Test]
        public void SpecificExceptionHandled()
        {
            Test<InvalidOperationException>(x => x.SpecificExceptionHandled());
        }
        [Test]
        public void SpecificExceptionHandledBySuper()
        {
            Test<InvalidOperationException>(x => x.SpecificExceptionHandledBySuper());
        }
        [Test]
        public void ExceptionUnhandled()
        {
            controller.ForceOnException(Assert.Throws<Exception>(() => controller.ExceptionUnhandled()));
            Assert.IsNull(controller.ViewData["Exception"]);
        }

        [Test]
        public void ExceptionWithNoHandledExceptions()
        {
            controller.ForceOnException(Assert.Throws<Exception>(() => controller.ExceptionWithNoHandledExceptions()));
            Assert.IsNull(controller.ViewData["Exception"]);
        }

        [Test]
        public void ShouldFailUnitOfWork()
        {
            controller.ForceOnException(Assert.Throws<Exception>(() => controller.HandleAndRollback()));
            Assert.IsNotNull(controller.ViewData["UOWException"]);
            Assert.IsNotNull(controller.ViewData["Exception"]);
        }

        [Test]
        public void NoError()
        {
            controller.NoError();
            Assert.IsTrue((bool)controller.ViewData["Executed"]);
        }
    }
}
