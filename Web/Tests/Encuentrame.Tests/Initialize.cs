using Microsoft.VisualStudio.TestTools.UnitTesting;
using NailsFramework.Logging;
using NailsFramework.TestSupport;
using NailsFramework.UserInterface;
using Encuentrame.Security.Authentications;
using Encuentrame.Tests.Supports;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Tests
{
    [TestClass]
    public static class Initialize
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            NailsTestsConfigurationRepository.Instance.Set("BaseTestConfiguration", (nails) =>
            {
                nails
                    .IoC.Container<NailsFramework.IoC.Spring>()
                    .Persistence.DataMapper<NailsFramework.Persistence.NHibernate>(
                        x => x.Configure(c => MappingConfigurator.Configure(c)))
                    .UserInterface.Platform<NullUIPlatform>()
                    .Logging.Logger<Log4net>()
                    .InspectAssembly(@"Encuentrame.Model.dll")
                    .InspectAssembly(@"Encuentrame.Support.Mappings.dll")
                    .InspectAssembly(@"Encuentrame.Model.Mappings.dll")
                    .InspectAssembly(@"Encuentrame.Tests.Supports.dll")
                    .InspectAssembly(@"Encuentrame.Tests.dll")
                    .InspectAssembly(@"Encuentrame.Support.Email.dll")
                    .IoC.Lemming<DomainAuthenticationProvider>()
                        .Lemming<AuthenticationDataProvider>()
                        .Lemming<TranslationService>()
                    .Initialize();

            });
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {

        }
    }
}
