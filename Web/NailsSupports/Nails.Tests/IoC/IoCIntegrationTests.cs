using NailsFramework.Mvp;
using NailsFramework.Tests.IoC.Lemmings;
using NailsFramework.Tests.IoC.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.IoC
{
    [TestFixture("Spring")]
    [TestFixture("Unity")]
    public class IoCIntegrationTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Nails.Reset(false);
        }

        #endregion

        private readonly string ioc;

        public IoCIntegrationTests(string ioc)
        {
            this.ioc = ioc;
        }

        [Test]
        public void ConfigureNails()
        {
            Nails.Configure()
                .IoC.Container(IoCContainers.GetContainer(ioc))
                .Lemming<ServiceDependency>()
                .Lemming<Service>()
                .Persistence.DataMapper<NailsFramework.Persistence.NHibernate>()
                .UserInterface.Platform<NullMvp>()
                .Initialize();
        }
    }
}