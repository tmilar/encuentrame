using System;
using NailsFramework.Tests.Support;
using NailsFramework.UserInterface.Binding;
using NUnit.Framework;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    [TestFixture]
    public class PresenterBindingTest : BaseTest
    {
        [Test]
        public void DefaultBindingIsByProperty()
        {
            Nails.Configure().IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .Initialize();

            var form = new TestFormPresenterProperty();
            Assert.IsNotNull(form.Presenter);
        }

        [Test]
        public void ViewFormFieldBinding()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>()
                .Initialize();

            var form = new TestFormPresenterField();
            Assert.IsNotNull(form.GetPresenter());
        }

        [Test]
        public void ViewFormPresenterFieldNameOverride()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>(l => l.Value(x => x.DefaultPresenterField, "_presenter"))
                .Initialize();

            var form = new TestFormWithOtherPresenterField();
            Assert.IsNotNull(form.GetPresenter());
        }

        [Test]
        public void ViewFormPresenterPropertyNameOverride()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterPropertyBinder>(l => l.Value(x => x.DefaultPresenterProperty, "PresenterValue"))
                .Initialize();

            var form = new TestFormWithOtherPresenterProperty();
            Assert.IsNotNull(form.PresenterValue);
        }

        [Test]
        public void ViewFormPropertyBinding()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterPropertyBinder>()
                .Initialize();

            var form = new TestFormPresenterProperty();
            Assert.IsNotNull(form.Presenter);
        }

        [Test]
        public void ViewFormThrowsWhenFieldNotFound()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>(l => l.Value(x => x.DefaultPresenterField, "xxxx"))
                .Initialize();

            Assert.Throws<InvalidOperationException>(() => new TestFormPresenterField());
        }

        [Test]
        public void ViewFormThrowsWhenPropertyNotFound()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterPropertyBinder>(l => l.Value(x => x.DefaultPresenterProperty, "xxxx"))
                .Initialize();

            Assert.Throws<InvalidOperationException>(() => new TestFormPresenterProperty());
        }

        [Test]
        public void ViewUserControlPresenterFieldNameOverride()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>(l => l.Value(x => x.DefaultPresenterField, "_presenter"))
                .Initialize();

            var form = new TestUserControlWithOtherPresenterField();
            Assert.IsNotNull(form.GetPresenter());
        }

        [Test]
        public void ViewUserControlPresenterPropertyNameOverride()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterPropertyBinder>(l => l.Value(x => x.DefaultPresenterProperty, "PresenterValue"))
                .Initialize();

            var form = new TestUserControlWithOtherPresenterProperty();
            Assert.IsNotNull(form.PresenterValue);
        }

        [Test]
        public void ViewUserControlThrowsWhenFieldNotFound()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>(l => l.Value(x => x.DefaultPresenterField, "xxxxxx"))
                .Initialize();

            Assert.Throws<InvalidOperationException>(() => new TestUserControlField());
        }

        [Test]
        public void ViewUserControlThrowsWhenPropertyNotFound()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterPropertyBinder>(l => l.Value(x => x.DefaultPresenterProperty, "xxxxxx"))
                .Initialize();

            Assert.Throws<InvalidOperationException>(() => new TestUserControlProperty());
        }

        [Test]
        public void ViewUserFieldBinding()
        {
            Nails.Configure()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .Lemming<PresenterFieldBinder>()
                .Initialize();

            var form = new TestUserControlField();
            Assert.IsNotNull(form.GetPresenter());
        }

        [Test]
        public void ViewUserPropertyBinding()
        {
            Nails.Configure().IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestPresenter>()
                .UserInterface.Platform<NailsFramework.UserInterface.WinForms>()
                .Initialize();

            var form = new TestUserControlProperty();
            Assert.IsNotNull(form.Presenter);
        }
    }
}