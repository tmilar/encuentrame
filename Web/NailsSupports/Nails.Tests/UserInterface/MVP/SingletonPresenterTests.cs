using System;
using NailsFramework.Mvp;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork.Session;
using NUnit.Framework;

namespace NailsFramework.Tests.UserInterface.MVP
{
    [TestFixture]
    public class SingletonPresenterTests : BaseTest
    {
        private class SwitcheableExcecutionContext : IExecutionContext
        {
            private readonly IExecutionContext a = new TestExecutionContext();
            private readonly IExecutionContext b = new TestExecutionContext();
            private IExecutionContext current;

            #region IExecutionContext Members

            public T GetObject<T>(string key)
            {
                return current.GetObject<T>(key);
            }

            public void SetObject(string key, object val)
            {
                current.SetObject(key, val);
            }

            public void RemoveObject(string key)
            {
                current.RemoveObject(key);
            }

            #endregion

            public void SwitchToA()
            {
                current = a;
            }

            public void SwitchToB()
            {
                current = b;
            }

            public void Dispose()
            {
                a.Dispose();
                b.Dispose();
            }
        }

        [Test]
        public void ShouldCohexistSeveralInstancesWithDifferentViewsInDifferentExecutionContexts()
        {
            Nails.Reset(false);

            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestSingletonPresenter>()
                .Lemming<SwitcheableExcecutionContext>()
                .UserInterface.Platform<NullMvp>()
                .Initialize();

            var context = Nails.ObjectFactory.GetObject<SwitcheableExcecutionContext>();

            var view1 = new TestSingletonView();
            var view2 = new TestSingletonView();

            context.SwitchToA();
            var presenter1 = PresenterProvider.GetFor(view1);
            context.SwitchToB();
            var presenter2 = PresenterProvider.GetFor(view2);

            Assert.AreSame(presenter1, presenter2);
            context.SwitchToA();
            Assert.AreEqual(presenter1.GetView(), view1);
            context.SwitchToB();
            Assert.AreEqual(presenter2.GetView(), view2);
        }

        [Test]
        public void ShouldNotCohexistSeveralInstancesWithDifferentViewsInTheSameExecutionContext()
        {
            Nails.Configure().IoC.Container<NailsFramework.IoC.Spring>()
                .Lemming<TestSingletonPresenter>()
                .UserInterface.Platform<NullMvp>()
                .Initialize();

            var view1 = new TestSingletonView();
            var view2 = new TestSingletonView();

            var presenter1 = PresenterProvider.GetFor(view1);

            var presenter2 = PresenterProvider.GetFor(view2);

            Assert.AreSame(presenter1, presenter2);
            Assert.AreEqual(presenter1.GetView(), view2); //the last one
        }
    }
}