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
    public class UnitOfWorkBehaviorTests : BaseTest
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

            workContextProvider.Expect(x => x.CurrentContext).Return(new WorkContext(persistenceContext));
            UnitOfWorkBehavior.ContextProvider = workContextProvider;

            behavior = new UnitOfWorkBehavior();
        }

        #endregion

        private IWorkContextProvider workContextProvider;
        private UnitOfWorkBehavior behavior;

        public string Test()
        {
            Assert.IsTrue(workContextProvider.CurrentContext.IsUnitOfWorkRunning);
            return "test";
        }

        [Test]
        public void InvocationShouldBeRanWithinAnUnitOfWork()
        {
            var method = GetType().GetMethod("Test");
            var invocation = new MethodInvocationInfo(Test, method);
            var result = behavior.ApplyTo(invocation);
            Assert.AreEqual("test", result);
        }
    }
}