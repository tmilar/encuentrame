using System.Linq;
using NailsFramework.Persistence;
using NailsFramework.Tests.Persistence.Common;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence
{
    public abstract class ExpectedCascadeBehaviorTestsBase<TCountry, TCity, TPersistenceTestHelper> : BaseTest
        where TCountry : class, ICountry<TCity>, new()
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
            Cities = Nails.ObjectFactory.GetObject<IBag<TCity>>();

            if (!connectionBoundUnitOfWork)
                persistenceContext.OpenSession();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (!Nails.Configuration.ConnectionBoundUnitOfWork)
                persistenceContext.CloseSession();
        }

        #endregion

        private readonly PersistenceTestHelper persistence;

        public ExpectedCascadeBehaviorTestsBase()
        {
            persistence = new TPersistenceTestHelper();
        }

        private IPersistenceContext persistenceContext;

        private IBag<TCountry> Countries { get; set; }
        private IBag<TCity> Cities { get; set; }
 
        [Test]
        public void ShouldSaveNewItemsInCollections()
        {
            SetUp(false);
            RunInUnitOfWork(() =>
            {
                Countries.Put(new TCountry { Name = "Argentina" });
                Countries.Put(new TCountry { Name = "USA" });
            });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var usa = Countries.GetCountryNamed("USA");
                argentina.AddCity("Buenos Aires");
                usa.AddCity("Chicago");
                usa.AddCity("New York");
            });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var usa = Countries.GetCountryNamed("USA");

                var argentinaCities = argentina.QueryCities();
                var usaCities = usa.QueryCities();

                Assert.AreEqual(1, argentinaCities.Count());
                Assert.AreEqual(2, usaCities.Count());
                Assert.AreEqual(3, Cities.Count());
            });
        }

        [Test]
        public void ShouldRemoveItemsInCollections()
        {
            SetUp(false);
            RunInUnitOfWork(() =>
            {
                Countries.Put(new TCountry { Name = "Argentina" });
                Countries.Put(new TCountry { Name = "USA" });
            });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var usa = Countries.GetCountryNamed("USA");
                argentina.AddCity("Buenos Aires");
                usa.AddCity("Chicago");
                usa.AddCity("New York");
            });

            RunInUnitOfWork(() =>
                                {
                                    var usa = Countries.GetCountryNamed("USA");
                                    usa.RemoveCity(Cities.GetCityNamed("Chicago"));
                                });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var usa = Countries.GetCountryNamed("USA");

                var argentinaCities = argentina.QueryCities();
                var usaCities = usa.QueryCities();

                Assert.AreEqual(1, argentinaCities.Count());
                Assert.AreEqual(1, usaCities.Count());
            });
        }

        [Test]
        public void ShouldUpdateItemsInCollections()
        {
            SetUp(false);
            RunInUnitOfWork(() =>
            {
                Countries.Put(new TCountry { Name = "Argentina" });
                Countries.Put(new TCountry { Name = "USA" });
            });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var usa = Countries.GetCountryNamed("USA");
                argentina.AddCity("Buenos Aires");
                usa.AddCity("Chicago");
                usa.AddCity("New York");
            });

            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");
                var buenosAires =  argentina.QueryCities().GetCityNamed("Buenos Aires");
                buenosAires.Name = "Bs As";
            });
            RunInUnitOfWork(() =>
            {
                var argentina = Countries.GetCountryNamed("Argentina");

                Assert.IsNotNull(Cities.GetCityNamed("Bs As"));
                Assert.IsNull(Cities.GetCityNamed("Buenos aires"));
                Assert.IsNotNull(argentina.QueryCities().GetCityNamed("Bs As"));
                Assert.IsNull(argentina.QueryCities().GetCityNamed("Buenos aires"));
            });
        }
    }
}