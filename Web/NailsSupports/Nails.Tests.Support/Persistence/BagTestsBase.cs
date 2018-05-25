using System;
using System.Linq;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    public abstract class BagTestsBase<TCountry, TPersistenceTestHelper> : BaseTest 
        where TCountry : class, ICountry, new()
        where TPersistenceTestHelper : PersistenceTestHelper, new()
    {
        #region Setup/Teardown

        public void SetUp(bool connectionBoundUnitOfWork)
        {
            var dataMapper = persistence.CreateDataMapper();

            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Persistence.DataMapper(dataMapper)
                .UnitOfWork.ConnectionBoundUnitOfWork(connectionBoundUnitOfWork)
                .Initialize();

            Nails.ObjectFactory.Inject(persistence);

            persistenceContext = Nails.ObjectFactory.GetObject<IPersistenceContext>();

            persistence.CreateDatabase();
          
            Countries = Nails.ObjectFactory.GetObject<IBag<TCountry>>();
         
            if (!connectionBoundUnitOfWork)
                persistenceContext.OpenSession();
        }
        
            
        [TearDown]
        public void TearDown()
        {
            if(!Nails.Configuration.ConnectionBoundUnitOfWork)
                persistenceContext.CloseSession();
        }

        #endregion

        private readonly PersistenceTestHelper persistence;

        public BagTestsBase()
        {
            persistence = new TPersistenceTestHelper();
        }

        private IPersistenceContext persistenceContext;

        protected IBag<TCountry> Countries { get; private set; }

        protected object PutSampleItemAndGetId()
        {
            var argentina = new TCountry {Name = "Argentina"};
            RunInUnitOfWork(() => Countries.Put(argentina));
            return argentina.Id;
        }

        private class ExpectedException : Exception
        {
        }

        [Test]
        public void GetAllInsideNonTransactionalUnitOfWorkShouldWork()
        {
            SetUp(true);
            PutSampleItemAndGetId();
            var items = RunInUnitOfWork(() => Countries.ToList(), false);
            Assert.IsNotNull(items);
            Assert.AreEqual(1, items.Count());
        }

        [Test]
        public void GetAllOutsideAnUnitOfWorkShouldWithUnitOfWorkBoundToConnectionShouldFail()
        {
            SetUp(true);
            Assert.Throws<InvalidOperationException>(() => Countries.ToList());
        }
        
        [Test]
        public void GetAllOutsideAnUnitOfWorkShouldWithUnitOfWorkNotBoundToConnection()
        {
            SetUp(false);
            Countries.ToList();
        }

        [Test]
        public void GetByIdInsideNonTransactionalUnitOfWorkShouldWork()
        {
            SetUp(false);
            var id = PutSampleItemAndGetId();
            var item = RunInUnitOfWork(() => Countries[id], false);
            Assert.IsNotNull(item);
            Assert.AreEqual(id, item.Id);
        }

        [Test]
        public void GetByIdOutsideAnUnitOfWorkShouldFail()
        {
            SetUp(false);
            var id = PutSampleItemAndGetId();
            Assert.Throws<UnitOfWorkViolationException>(() => { var ignore = Countries[id]; });
        }

        [Test]
        public void PutGetAll()
        {
            SetUp(false);
            var argentina = new TCountry
                                {
                                    Name = "Argentina"
                                };

            var usa = new TCountry
                          {
                              Name = "USA"
                          };

            RunInUnitOfWork(() =>
                                {
                                    Countries.Put(argentina);
                                    Countries.Put(usa);
                                });
            RunInUnitOfWork(() => Assert.AreEqual(2, Countries.ToList().Count()));
        }

        [Test]
        public void PutGetByIdRemove()
        {
            SetUp(false);
            var argentina = new TCountry
                                {
                                    Name = "Argentina"
                                };

            RunInUnitOfWork(() => Countries.Put(argentina));
            RunInUnitOfWork(() =>
                                {
                                    var country = Countries[argentina.Id];
                                    Assert.AreEqual(argentina.Name, country.Name);
                                    Countries.Remove(country);
                                });
            RunInUnitOfWork(() =>
                                {
                                    var country = Countries.ToList().SingleOrDefault(x => x.Id == argentina.Id);
                                    Assert.IsNull(country);
                                });
        }

        [Test]
        public void PutInsideNonTransactionalUnitOfWorkShouldFail()
        {
            SetUp(false);
            var item = new TCountry {Name = "Argentina"};
            Assert.Throws<UnitOfWorkViolationException>(() => RunInUnitOfWork(() => Countries.Put(item), false));
        }

        [Test]
        public void PutOutsideAnUnitOfWorkShouldFail()
        {
            SetUp(false);
            var item = new TCountry {Name = "Argentina"};
            Assert.Throws<UnitOfWorkViolationException>(() => Countries.Put(item));
        }

        [Test]
        public void Query()
        {
            SetUp(false);
            var argentina = new TCountry
                                {
                                    Name = "Argentina"
                                };

            var usa = new TCountry
                          {
                              Name = "USA"
                          };

            RunInUnitOfWork(() =>
                                {
                                    Countries.Put(argentina);
                                    Countries.Put(usa);
                                });

            var country = RunInUnitOfWork(() => Countries.Where(x => x.Name == "Argentina").SingleOrDefault());
            Assert.IsNotNull(country);
            Assert.AreEqual("Argentina", country.Name);
        }

        [Test]
        public void RemoveInsideNonTransactionalUnitOfWorkShouldFail()
        {
            SetUp(false);
            var item = new TCountry {Name = "Argentina"};
            RunInUnitOfWork(() => Countries.Put(item));
            Assert.Throws<UnitOfWorkViolationException>(() => RunInUnitOfWork(() => Countries.Remove(item), false));
        }

        [Test]
        public void RemoveOutsideAnUnitOfWorkShouldFail()
        {
            SetUp(false);
            var item = new TCountry {Name = "Argentina"};
            RunInUnitOfWork(() => Countries.Put(item));
            Assert.Throws<UnitOfWorkViolationException>(() => Countries.Remove(item));
        }

        [Test]
        public void Rollback()
        {
            SetUp(false);

            Assert.Throws<ExpectedException>(() => RunInUnitOfWork(() =>
                                                                       {
                                                                           var argentina = new TCountry { Name = "Argentina" };
                                                                           Countries.Put(argentina);
                                                                           throw new ExpectedException();
                                                                       }));
            var item = RunInUnitOfWork(() => Countries.SingleOrDefault(x => x.Name == "Argentina"));
            Assert.IsNull(item);
        }
    }
}