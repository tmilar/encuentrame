using System.Linq;
using System.Reflection;
using NailsFramework.Mvp;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NUnit.Framework;

namespace NailsFramework.Tests.UserInterface.MVP
{
    [TestFixture]
    public class MVPPlatformTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        #endregion

        private readonly MethodInfo testMethod = typeof (ITestPresenter).GetMethod("Test");
        private readonly MethodInfo setViewMethod = typeof (IPresenter).GetMethod("set_View");

        [Test]
        public void AspectsForPresenterMethodsExceptSetView()
        {
            Nails.Configure().UserInterface.Platform<NullMvp>()
                .Initialize(configureDefaults: false);

            var aspects = Nails.Configuration.Aspects;

            Assert.That(aspects.All(x => x.AppliesTo(testMethod)));

            Assert.That(aspects.All(x => !x.AppliesTo(setViewMethod)));
        }

        [Test]
        public void UnitOfWorkBehavior()
        {
            Nails.Configure().UserInterface.Platform<NullMvp>()
                .Initialize(configureDefaults: false);

            var aspects = Nails.Configuration.Aspects;
            Assert.That(aspects.Any(x => x.Behavior is UnitOfWorkBehavior && x.AppliesTo(testMethod)));
        }
    }
}