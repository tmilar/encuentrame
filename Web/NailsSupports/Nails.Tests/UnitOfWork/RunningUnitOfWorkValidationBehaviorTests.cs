using NailsFramework.Aspects;
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
    public class RunningUnitOfWorkValidationBehaviorTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Nails.Configure()
                .IoC.Container<NullIoCContainer>()
                .Initialize();

            workContextProvider = MockRepository.GenerateMock<IWorkContextProvider>();
            var persistenceContext = MockRepository.GenerateMock<IPersistenceContext>();
            var transactionalContext = MockRepository.GenerateMock<ITransactionalContext>();
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext);

            workContext = new WorkContext(persistenceContext);
            workContextProvider.Expect(x => x.CurrentContext).Return(workContext);
            RunningUnitOfWorkValidationBehavior.WorkContextProvider = workContextProvider;
            behavior = new RunningUnitOfWorkValidationBehavior();
            behavior.SetTransactionalMethods<RunningUnitOfWorkValidationBehaviorTests>("TransactionalTest");
        }

        #endregion

        private IWorkContextProvider workContextProvider;
        private RunningUnitOfWorkValidationBehavior behavior;
        private WorkContext workContext;

        public string Test()
        {
            return "test";
        }

        public string TransactionalTest()
        {
            return "transactional-test";
        }

        [Test]
        public void ShouldNotThrowWhenCallingNonTransactionalMethodWithinANonTransactionalUnitOfWork()
        {
            var method = GetType().GetMethod("Test");
            var invocation = new MethodInvocationInfo(Test, method);
            workContext.RunUnitOfWork(() => behavior.ApplyTo(invocation), new UnitOfWorkInfo(false));
        }

        [Test]
        public void ShouldNotThrowWhenCallingNonTransactionalMethodWithinATransactionalUnitOfWork()
        {
            var method = GetType().GetMethod("Test");
            var invocation = new MethodInvocationInfo(Test, method);
            workContext.RunUnitOfWork(() => behavior.ApplyTo(invocation), new UnitOfWorkInfo(true));
        }

        [Test]
        public void ShouldNotThrowWhenCallingTransactionalMethodWithinATransactionalUnitOfWork()
        {
            var method = GetType().GetMethod("TransactionalTest");
            var invocation = new MethodInvocationInfo(TransactionalTest, method);
            var result = workContext.RunUnitOfWork(() => behavior.ApplyTo(invocation), new UnitOfWorkInfo(true));
            Assert.AreEqual("transactional-test", result);
        }

        [Test]
        public void ShouldThrowExceptionWhenCallingNonTransactionalMethodOutsideAUnitOfWork()
        {
            var method = GetType().GetMethod("Test");
            var invocation = new MethodInvocationInfo(Test, method);
            Assert.Throws<UnitOfWorkViolationException>(() => behavior.ApplyTo(invocation));
        }

        [Test]
        public void ShouldThrowExceptionWhenCallingTransactionalMethodOutsideAUnitOfWork()
        {
            var method = GetType().GetMethod("TransactionalTest");
            var invocation = new MethodInvocationInfo(TransactionalTest, method);
            Assert.Throws<UnitOfWorkViolationException>(() => behavior.ApplyTo(invocation));
        }

        [Test]
        public void ShouldThrowWhenCallingTransactionalMethodWithinANonTransactionalUnitOfWork()
        {
            var method = GetType().GetMethod("TransactionalTest");
            var invocation = new MethodInvocationInfo(TransactionalTest, method);
            Assert.Throws<UnitOfWorkViolationException>(
                () => workContext.RunUnitOfWork(() => behavior.ApplyTo(invocation),
                                                new UnitOfWorkInfo(false)));
        }
    }
}