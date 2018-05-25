using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.UserInterface.MVP.WebForms
{
    [TestFixture]
    public class PresenterBindingTest : BaseTest
    {
        [Test]
        public void ViewFormDefaultBinding()
        {
            Nails.Configure().IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .UserInterface.Platform<NailsFramework.UserInterface.WebForms>()
                .Initialize();

            var form = new TestPage();
            Assert.IsNotNull(form.GetPresenter());
        }

        [Test]
        public void ViewUserControlDefaultBinding()
        {
            Nails.Configure().IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .UserInterface.Platform<NailsFramework.UserInterface.WebForms>()
                .Initialize();

            var form = new TestUserControl();
            Assert.IsNotNull(form.GetPresenter());
        }

        public override bool UseTestContext { get { return false; } }
    }
}