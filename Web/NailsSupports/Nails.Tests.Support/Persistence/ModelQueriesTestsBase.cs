using System;
using System.Linq;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    public class ModelQueriesTestsBase<TCountry, TCity, TPersistenceTestHelper> : BaseTest
        where TCountry : Model<TCountry>, ICountry<TCity>, new() 
        where TCity : class, ICity 
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

        protected IBag<TCountry> Countries { get; private set; }
        private readonly TPersistenceTestHelper persistence;
        private IPersistenceContext persistenceContext;

        public ModelQueriesTestsBase()
        {
            persistence = new TPersistenceTestHelper();
        }

        protected void PutItems()
        {
            RunInUnitOfWork(() =>
                                {
                                    Countries.Put(new TCountry {Name = "Argentina"});
                                       Countries.Put(new TCountry {Name = "USA"});
                                });
            RunInUnitOfWork(() =>
                                {
                                    var argentina = Countries.GetCountryNamed("Argentina");
                                    var usa = Countries.GetCountryNamed("USA");
                                    argentina.AddCity("Cordoba");
                                    argentina.AddCity("Buenos Aires");
                                    argentina.AddCity("Catamarca");
                                    usa.AddCity("California");
                                    usa.AddCity("Chicago");
                                    usa.AddCity("New York");
                                    usa.AddCity("Colorado");
                                });
        }

        [Test]
        public void QueryingForExistingElements()
        {
            SetUp(true);
            PutItems();
            var cities = RunInUnitOfWork(() =>
                                             {
                                                 var country = Countries.GetCountryNamed("Argentina");
                                                 return country.CitiesStartingWith("C").ToList();
                                             });
            Assert.AreEqual(2, cities.Count);
        }

        [Test]
        public void QueryingForNonExistingElements()
        {
            SetUp(true);
            PutItems();
            var cities = RunInUnitOfWork(() =>
                                             {
                                                 var country = Countries.GetCountryNamed("Argentina");
                                                 return country.CitiesStartingWith("dk.3").ToList();
                                             });
            Assert.AreEqual(0, cities.Count);
        }

        [Test]
        public void QueryingOutsideUnitOfWorkWithConnectionBoundUnitOfWorkShouldFail()
        {
            SetUp(true);
            PutItems();
            var argentina = RunInUnitOfWork(() => Countries.GetCountryNamed("Argentina"));
            Assert.Throws<InvalidOperationException>(() => argentina.CitiesStartingWith("C").ToList());
        }

        [Test]
        public void QueryingOutsideUnitOfWorkWithConnectionUnboundUnitOfWork()
        {
            SetUp(false);
            
            PutItems();
            var argentina = RunInUnitOfWork(() => Countries.GetCountryNamed("Argentina"));
            var cities = argentina.CitiesStartingWith("C").ToList();
            Assert.AreEqual(2, cities.Count);
        }

        [Test]
        public void GetByIdShouldReturnFromACollection()
        {
            if(!persistence.AddsIdsToCollectionItems)
                return;
            SetUp(false);

            PutItems();
            var argentina = RunInUnitOfWork(() => Countries.GetCountryNamed("Argentina"));
            var argentinanCity = argentina.QueryCities().First();
            var city = argentina.QueryCities().GetById(argentinanCity.Id);
            Assert.AreEqual(argentinanCity.Name, city.Name);
        }
        [Test]
        public void GetByIdShouldNotReturnFromAnotherCollection()
        {
            if (!persistence.AddsIdsToCollectionItems)
                return;
            SetUp(false);

            PutItems();
            var argentina = RunInUnitOfWork(() => Countries.GetCountryNamed("Argentina"));
            var usa = RunInUnitOfWork(() => Countries.GetCountryNamed("USA"));
            var argentinanCity = argentina.QueryCities().First();
            var city = usa.QueryCities().GetById(argentinanCity.Id);
            Assert.IsNull(city);
        }
    }
}