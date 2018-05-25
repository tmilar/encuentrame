using System;
using System.Linq;
using NailsFramework.Config;
using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.Config
{
    [TestFixture]
    public class ConfigurationTests : BaseTest
    {
        [Test]
        public void AssembliesToInspect()
        {
            Nails.Configure().InspectAssembly("Nails.Tests.dll")
                .InspectAssembly("Nails.dll");

            var assembliesToInspect = new AssembliesToInspect();

            Assert.AreEqual(2, assembliesToInspect.Count());
        }

        [Test]
        public void ConnectionBoundUnitOfWorkByDefault()
        {
            Assert.IsTrue(Nails.Configuration.ConnectionBoundUnitOfWork);
        }

        [Test]
        public void DontAllowAsyncExecutionByDefault()
        {
            Assert.IsFalse(Nails.Configuration.DefaultAsyncMode);
        }

        [Test]
        public void InspectAssemblies()
        {
            Nails.Configure().InspectAssembly(GetType().Assembly)
                .InspectAssembly(typeof (Nails).Assembly);

            Assert.AreEqual(2, Nails.Configuration.AssembliesToInspect.Count());
        }

        [Test]
        public void InspectAssembliesByName()
        {
            Nails.Configure().InspectAssembly("Nails.Tests.dll")
                .InspectAssembly("Nails.dll");

            Assert.AreEqual(2, Nails.Configuration.AssembliesToInspect.Count());
        }

        [Test]
        public void InspectAssembliesByTypes()
        {
            Nails.Configure().InspectAssemblyOf<ConfigurationTests>()
                .InspectAssemblyOf<NailsConfiguration>();

            Assert.AreEqual(2, Nails.Configuration.AssembliesToInspect.Count());
        }

        [Test]
        public void NotAsyncModeByDefault()
        {
            Assert.IsFalse(Nails.Configuration.DefaultAsyncMode);
        }

        [Test]
        public void ShouldNotLetConfigureNailsOnceInitialized()
        {
            Nails.Configure()
                .Initialize();

            Assert.Throws<InvalidOperationException>(() => Nails.Configure());
        }

        [Test]
        public void ShouldSetAllowAsyncExecution()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(true);
            Assert.IsTrue(Nails.Configuration.DefaultAsyncMode);
        }

        [Test]
        public void ShouldSetConnectionBoundUnitOfWork()
        {
            Nails.Configure().UnitOfWork.ConnectionBoundUnitOfWork(false);
            Assert.IsFalse(Nails.Configuration.ConnectionBoundUnitOfWork);
        }

        [Test]
        public void ShouldSetDefaultAsyncMode()
        {
            Nails.Configure().UnitOfWork.DefaultAsyncMode(false);
            Assert.IsFalse(Nails.Configuration.DefaultAsyncMode);
        }

        [Test]
        public void ShouldSetDefaultTransactionMode()
        {
            Nails.Configure().UnitOfWork.DefaultTransactionMode(TransactionMode.NoTransaction);
            Assert.AreEqual(TransactionMode.NoTransaction, Nails.Configuration.DefaultTransactionMode);
        }

        [Test]
        public void ShouldSetPageSize()
        {
            Nails.Configure().Persistence.PageSize(10);
            Assert.AreEqual(10, Nails.Configuration.PageSize);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void OnConfigurationErrorShouldThrowException()
        {
            DataMapper dataMapper = CreateDataMapper();
            dataMapper.Expect(x => x.Initialize()).Throw(new Exception());
            
            Nails.Configure().Persistence.DataMapper(dataMapper).Initialize();
        }

        [Test]
        public void OnConfigurationErrorShouldAddMissingConfiguration()
        {
            DataMapper dataMapper = CreateDataMapper();
            dataMapper.Expect(x => x.Initialize()).Throw(new Exception());

            Assert.Throws<Exception>(() => Nails.Configure().Persistence.DataMapper(dataMapper).Initialize());
            CollectionAssert.IsNotEmpty(Nails.Status.MissingConfigurations);
        }

        [Test]
        public void OnConfigurationErrorShouldNotBeReady()
        {
            DataMapper dataMapper = CreateDataMapper();
            dataMapper.Expect(x => x.Initialize()).Throw(new Exception());

            Assert.Throws<Exception>(() => Nails.Configure().Persistence.DataMapper(dataMapper).Initialize());
            Assert.IsFalse(Nails.Status.IsReady);
        }

        private DataMapper CreateDataMapper()
        {
            var mocks = new MockRepository();
            var dataMapper = mocks.DynamicMock<DataMapper>();
            dataMapper.Expect(x => x.BagType).Return(typeof(NullBag<>));
            dataMapper.Expect(x => x.PersistenceContextType).Return(typeof(NullPersistenceContext));
            dataMapper.Replay();
            return dataMapper;
        }
    }
}