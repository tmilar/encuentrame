using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.Session;
using NUnit.Framework;

namespace NailsFramework.Tests.UnitOfWork.Session
{
    [TestFixture]
    public class UnitOfWorkSessionContextTests : BaseTest
    {
        [SetUp]
        public void SetUp()
        {
            Nails.Reset(false);
            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Initialize();

            sessionContext = Nails.ObjectFactory.GetObject<ISessionContext>();
        }

        private ISessionContext sessionContext;

        [Test]
        public void GetAndSetFromSessionContextInsideAUnitOfWorkShouldWork()
        {
            var value = 0;
            RunInUnitOfWork(() =>
                                {
                                    sessionContext.SetObject("bla", 1234);
                                    value = sessionContext.GetObject<int>("bla");
                                });
            Assert.AreEqual(1234, value);
        }

        [Test]
        public void GetFromSessionContextOutsideAUnitOfWorkShouldFail()
        {
            Assert.Throws<UnitOfWorkViolationException>(() => sessionContext.GetObject<object>("bla"));
        }

        [Test]
        public void SetFromSessionContextOutsideAUnitOfWorkShouldFail()
        {
            Assert.Throws<UnitOfWorkViolationException>(() => sessionContext.SetObject("bla", 1234));
        }
    }
}