using NailsFramework.IoC;
using NailsFramework.Mvp;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.UserInterface.MVP
{
    [TestFixture]
    public class PresenterProviderTests : BaseTest
    {
        [Test]
        public void ShouldReturnPresenterForTheDefinedGenericView()
        {
            var mocks = new MockRepository();
            var objectFactory = mocks.DynamicMock<IObjectFactory>();
            var presenter = new TestPresenter();
            objectFactory.Expect(x => x.GetObject<ITestPresenter>()).Return(presenter);

            mocks.ReplayAll();
            var view = new TestView();
            PresenterProvider.ObjectFactory = objectFactory;
            var providedPresenter = PresenterProvider.GetFor(view);
            mocks.VerifyAll();
            Assert.AreEqual(presenter, providedPresenter);
            Assert.AreEqual(view, presenter.GetView());
        }

        [Test]
        public void ShouldReturnPresenterForTheDefinedView()
        {
            var mocks = new MockRepository();
            var objectFactory = mocks.DynamicMock<IObjectFactory>();
            var presenter = new TestPresenter();
            objectFactory.Expect(x => x.GetObject(typeof (ITestPresenter))).Return(presenter);

            mocks.ReplayAll();
            IView view = new TestView();
            PresenterProvider.ObjectFactory = objectFactory;
            var providedPresenter = PresenterProvider.GetFor(view);
            mocks.VerifyAll();
            Assert.AreEqual(presenter, providedPresenter);
            Assert.AreEqual(view, presenter.GetView());
        }
    }
}