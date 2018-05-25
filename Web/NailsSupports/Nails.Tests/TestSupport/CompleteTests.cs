using NailsFramework.Config;
using NailsFramework.IoC;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.TestSupport;
using NUnit.Framework;

namespace NailsFramework.Tests.TestSupport
{
    [TestFixture]
    public class CompleteTests : NailsTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #endregion

        [Inject("AppSettingsValue")]
        public string ValueFromConfiguration { private get; set; }

        [Inject]
        public IService Reference { private get; set; }

        protected override void ConfigureNails(INailsConfigurator nails)
        {
            nails.IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<Service>()
                .Lemming<ServiceDependency>()
                .Initialize();
        }

        [Test]
        public void ShouldHaveCurrentUnitOfWork()
        {
            Assert.IsNotNull(CurrentUnitOfWork);
        }

        [Test]
        public void ShouldHaveWorkContextProvider()
        {
            Assert.IsNotNull(WorkContextProvider);
        }

        [Test]
        public void ShouldInjectReferences()
        {
            Assert.IsNotNull(Reference);
        }

        [Test]
        public void ShouldInjectValuesFromConfiguration()
        {
            Assert.AreEqual("test123", ValueFromConfiguration);
        }

        [Test]
        public void ShouldNotBeInsideAUnitOfWorkByDefault()
        {
            Assert.IsFalse(WorkContextProvider.CurrentContext.IsUnitOfWorkRunning);
        }

        [Test]
        public void ShouldRunActionsInsideAUnitOfWork()
        {
            RunInUnitOfWork(() => Assert.IsTrue(WorkContextProvider.CurrentContext.IsUnitOfWorkRunning));
        }

        [Test]
        public void ShouldRunFuncsInsideAUnitOfWork()
        {
            var isUnitOfWorkRunning = RunInUnitOfWork(() => WorkContextProvider.CurrentContext.IsUnitOfWorkRunning);
            Assert.IsTrue(isUnitOfWorkRunning);
        }
    }
}