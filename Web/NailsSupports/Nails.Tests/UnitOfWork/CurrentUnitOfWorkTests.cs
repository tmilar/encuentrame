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
    public class CurrentUnitOfWorkTests : BaseTest
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
            var persistenceContext = mocks.DynamicMock<IPersistenceContext>();
            transactionalContext = mocks.DynamicMock<ITransactionalContext>();
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext);

            workContext = new WorkContext(persistenceContext);
            workContextProvider.Expect(x => x.CurrentContext).Return(workContext).Repeat.Any();

            currentUnitOfWork = new CurrentUnitOfWork
                                    {
                                        WorkContextProvider = workContextProvider
                                    };
        }

        #endregion

        private CurrentUnitOfWork currentUnitOfWork;
        private bool failureCall;
        private bool successCall;
        private MockRepository mocks;
        private IWorkContextProvider workContextProvider;
        private WorkContext workContext;

        private ITransactionalContext transactionalContext;

        private void Test(bool throwException)
        {
            currentUnitOfWork.OnSuccessCall(() => { successCall = true; });
            currentUnitOfWork.OnFailureCall(e => { failureCall = true; });
            if (throwException)
                throw new TestException();
        }

        [Test]
        public void ShouldCallCommitAndBeginAnotherTransactionWhenCallingCheckpointOnNonTransactionalUnitOfWork()
        {
            transactionalContext.Expect(x => x.Commit()).Repeat.Never();
            transactionalContext.Expect(x => x.Begin()).Repeat.Never();

            mocks.ReplayAll();

            workContext.RunUnitOfWork(currentUnitOfWork.Checkpoint, new UnitOfWorkInfo(false));

            transactionalContext.VerifyAllExpectations();
        }

        [Test]
        public void ShouldCallCommitAndBeginAnotherTransactionWhenCallingCheckpointOnTransactionalUnitOfWork()
        {
            transactionalContext.Expect(x => x.Commit()).Repeat.Once();
            transactionalContext.Expect(x => x.Begin()).Repeat.Once();

            mocks.ReplayAll();

            workContext.RunUnitOfWork(currentUnitOfWork.Checkpoint, new UnitOfWorkInfo(true));

            transactionalContext.VerifyAllExpectations();
        }

        [Test]
        public void ShouldCallCurrentUnitOfWorkOnFailureCall()
        {
            mocks.ReplayAll();

            Assert.Throws<TestException>(
                () => workContext.RunUnitOfWork(() => Test(throwException: true), new UnitOfWorkInfo(false)));

            Assert.IsTrue(failureCall);
            Assert.IsFalse(successCall);
        }

        [Test]
        public void ShouldCallCurrentUnitOfWorkOnSuccessCall()
        {
            mocks.ReplayAll();

            workContext.RunUnitOfWork(() => Test(throwException: false), new UnitOfWorkInfo(false));

            Assert.IsFalse(failureCall);
            Assert.IsTrue(successCall);
        }

        [Test]
        public void ShouldThrowExceptionWhenCallingCurrentUnitOfWorkOutsideAUnitOfWork()
        {
            mocks.ReplayAll();

            Assert.Throws<NailsException>(currentUnitOfWork.Checkpoint);
            Assert.Throws<NailsException>(() => currentUnitOfWork.OnFailureCall(e => { }));
            Assert.Throws<NailsException>(() => currentUnitOfWork.OnSuccessCall(() => { }));
        }
    }
}