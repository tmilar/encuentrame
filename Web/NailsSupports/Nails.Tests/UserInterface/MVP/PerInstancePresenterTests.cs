using NailsFramework.Mvp;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.UserInterface.MVP
{
    [TestFixture]
    public class PerInstancePresenterTests : BaseTest
    {
        [Test]
        public void ShouldCohexistSeveralInstancesWithDifferentViews()
        {
            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .UserInterface.Platform<NullMvp>()
                .Initialize();

            var view1 = new TestView();
            var view2 = new TestView();

            var presenter1 = PresenterProvider.GetFor(view1);
            var presenter2 = PresenterProvider.GetFor(view2);

            Assert.AreNotEqual(presenter1, presenter2);
            Assert.AreEqual(presenter1.GetView(), view1);
            Assert.AreEqual(presenter2.GetView(), view2);
        }
    }
}