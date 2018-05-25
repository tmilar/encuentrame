using System.Reflection;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            failureCall = false;
            successCall = false;


            Nails.Configure()
                .IoC.Container<NullIoCContainer>()
                .Initialize();

            workContextProvider = mocks.DynamicMock<IWorkContextProvider>();
            persistenceContext = mocks.DynamicMock<IPersistenceContext>();
            transactionalContext = mocks.DynamicMock<ITransactionalContext>();

            workContext = new WorkContext(persistenceContext);
            workContextProvider.Expect(x => x.CurrentContext).Return(workContext).Repeat.Any();
        }

        #endregion

        private bool failureCall;
        private bool successCall;
        private MockRepository mocks;
        private IWorkContextProvider workContextProvider;
        private WorkContext workContext;
        private ITransactionalContext transactionalContext;
        private IPersistenceContext persistenceContext;

        private void TestSuscriptions(bool throwException)
        {
            workContext.CurrentUnitOfWork.Subscriptions.OnSuccessCall(() => { successCall = true; });
            workContext.CurrentUnitOfWork.Subscriptions.OnFailureCall(e => { failureCall = true; });
            if (throwException)
                throw new TestException("expected");
        }

        [Test]
        public void Cache()
        {
            mocks.ReplayAll();

            workContext.RunUnitOfWork(() =>
                                          {
                                              workContext.CurrentUnitOfWork.Cache.AddItem("test", "1234");
                                              var item = workContext.CurrentUnitOfWork.Cache.GetItem<string>("test");
                                              Assert.AreEqual("1234", item);
                                          }, new UnitOfWorkInfo(false));
        }

       
        [Test]
        public void IsIsUnitOfWorkRunningFalseWhenNotRunning()
        {
            mocks.ReplayAll();
            Assert.IsFalse(workContext.IsUnitOfWorkRunning);
        }

        [Test]
        public void IsIsUnitOfWorkRunningTrueWhenRunning()
        {
            mocks.ReplayAll();
            var running = false;
            workContext.RunUnitOfWork(() => { running = workContext.IsUnitOfWorkRunning; },
                                      new UnitOfWorkInfo(false));

            Assert.IsTrue(running);
        }

        [Test]
        public void OnFailureCall()
        {
            mocks.ReplayAll();

            Assert.Throws<TestException>(
                () =>
                workContext.RunUnitOfWork(() => TestSuscriptions(throwException: true),
                                          new UnitOfWorkInfo(false)));

            Assert.IsTrue(failureCall);
            Assert.IsFalse(successCall);
        }

        [Test]
        public void Cancel()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Once();

            mocks.ReplayAll();

            workContext.RunUnitOfWork(() => workContext.CurrentUnitOfWork.Cancel(), new UnitOfWorkInfo(true));

            persistenceContext.VerifyAllExpectations();
            transactionalContext.VerifyAllExpectations();
        }

        [Test]
        public void OnSuccessCall()
        {
            mocks.ReplayAll();

            workContext.RunUnitOfWork(() => TestSuscriptions(throwException: false), new UnitOfWorkInfo(false));

            Assert.IsFalse(failureCall);
            Assert.IsTrue(successCall);
        }

        [Test]
        public void ShouldBeginATransactionForTransactionalUnitOfWorks()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            workContext.RunUnitOfWork(() => { }, new UnitOfWorkInfo(true));

            persistenceContext.VerifyAllExpectations();
            transactionalContext.VerifyAllExpectations();
        }

        [Test]
        public void ShouldNotBeginATransactionForTransactionalUnitOfWorks()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Never();
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Begin()).Repeat.Never();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            workContext.RunUnitOfWork(() => { }, new UnitOfWorkInfo(false));

            transactionalContext.VerifyAllExpectations();
        }


        [Test]
        public void ShouldNotFireUnhandledExceptionIfNoExceptionWasThrown()
        {
            mocks.ReplayAll();
            workContext.UnhandledException += delegate { Assert.Fail("UnhandledException not expected"); };
            workContext.RunUnitOfWork(() => { }, new UnitOfWorkInfo(false));
        }

        [Test]
        public void ShouldNotRollbackButThrowOnFailureOnTransactionalUnitOfWork()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Never();
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Begin()).Repeat.Never();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            Assert.Throws<TestException>(
                () =>
                workContext.RunUnitOfWork(() => { throw new TestException("expected"); },
                                          new UnitOfWorkInfo(false)));
        }

        [Test]
        public void ShouldNotRollbackButThrowOnLenientExceptionInsideATargetInvotacionExceptionOnTransactionalUnitOfWork()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            var exception = new TargetInvocationException(new LenientException("expected"));

            Assert.Throws<LenientException>(
                () => workContext.RunUnitOfWork(() => { throw exception; }, new UnitOfWorkInfo(true)));
        }

        [Test]
        public void ShouldNotRollbackButThrowOnLenientExceptionOnTransactionalUnitOfWork()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            Assert.Throws<LenientException>(
                () =>
                workContext.RunUnitOfWork(() => { throw new LenientException("expected"); },
                                          new UnitOfWorkInfo(true)));
        }

        [Test]
        public void ShouldRollbackAndThrowOnExceptionInsideATargetInvotacionExceptionOnTransactionalUnitOfWork()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Once();

            mocks.ReplayAll();
            var exception = new TargetInvocationException(new TestException("expected"));
            Assert.Throws<TestException>(
                () => workContext.RunUnitOfWork(() => { throw exception; }, new UnitOfWorkInfo(true)));
        }

        [Test]
        public void ShouldRollbackAndThrowOnFailureOnTransactionalUnitOfWork()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Once();

            mocks.ReplayAll();

            Assert.Throws<TestException>(
                () =>
                workContext.RunUnitOfWork(() => { throw new TestException("expected"); },
                                          new UnitOfWorkInfo(true)));
        }

        [Test]
        public void ExecutionShouldSuccessWhenNoErrors()
        {
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            transactionalContext.Expect(x => x.Commit()).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();
            transactionalContext.Expect(x => x.Rollback()).Repeat.Never();

            mocks.ReplayAll();

            var execution = workContext.BeginUnitOfWork(new UnitOfWorkInfo());
            var unitOfWork = workContext.CurrentUnitOfWork;
            execution.End();

            Assert.AreEqual(UnitOfWorkStatus.Successfull, unitOfWork.Status);
        }
    }
}