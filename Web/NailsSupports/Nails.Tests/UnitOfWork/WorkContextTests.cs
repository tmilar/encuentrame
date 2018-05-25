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
    public class WorkContextTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            Nails.Configure()
                .IoC.Container<NullIoCContainer>()
                .Initialize();

            workContextProvider = mocks.DynamicMock<IWorkContextProvider>();
            persistenceContext = mocks.DynamicMock<IPersistenceContext>();
            transactionalContext = mocks.DynamicMock<ITransactionalContext>();

            workContext = new WorkContext(persistenceContext);
            persistenceContext.Expect(x => x.CreateTransactionalContext()).Return(transactionalContext).Repeat.Once();
            workContextProvider.Expect(x => x.CurrentContext).Return(workContext).Repeat.Any();
        }

        #endregion

        private MockRepository mocks;
        private IWorkContextProvider workContextProvider;
        private WorkContext workContext;
        private IPersistenceContext persistenceContext;
        private ITransactionalContext transactionalContext;

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
        public void NestedUnitOfWorksShouldBeTheSame()
        {
            mocks.ReplayAll();
            var areTheSame = false;
            workContext.RunUnitOfWork(() =>
                                          {
                                              var current = workContext.CurrentUnitOfWork;
                                              workContext.RunUnitOfWork(
                                                  () => { areTheSame = current == workContext.CurrentUnitOfWork; },
                                                  new UnitOfWorkInfo(false));
                                          }, new UnitOfWorkInfo(false));

            Assert.IsTrue(areTheSame);
        }

        [Test]
        public void _IsIsUnitOfWorkRunningTrueWhenRunning()
        {
            mocks.ReplayAll();
            var running = false;
            workContext.RunUnitOfWork(() => { running = workContext.IsUnitOfWorkRunning; },
                                      new UnitOfWorkInfo(true));

            Assert.IsTrue(running);
        }
    }
}