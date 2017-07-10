using System.Threading;
using NailsFramework.IoC;

namespace NailsFramework.TestSupport
{
    /// <summary>
    /// Responsible for applying the correct configuration for a given <see cref="NailsTestsConfigurable"/> test by using what's configured
    /// in the <see cref="NailsTestsConfigurationRepository"/>. 
    /// </summary>
    public sealed class NailsTestsConfigurator
    {
        public static readonly NailsTestsConfigurator Instance = new NailsTestsConfigurator();

        private string currentConfigurationName = null;
        private ReferenceResolver currentReferenceResolver = null;
        private ReaderWriterLock rwLock = null;
        public int ConfigurationTimeout { get; private set; }

        private NailsTestsConfigurator()
        {
            rwLock = new ReaderWriterLock();
            SetInifiteConfigurationTimeout();
        }

        public ReferenceResolver AcquireConfiguration(NailsTestsConfigurable test)
        {
            rwLock.AcquireReaderLock(ConfigurationTimeout);

            var testConfigurationName = test.ConfigurationName;

            if (currentConfigurationName != testConfigurationName)
            {
                var lockCookie = rwLock.UpgradeToWriterLock(ConfigurationTimeout);
                try
                {
                    var configurationDelegate = NailsTestsConfigurationRepository.Instance.Get(testConfigurationName);
                    Nails.Reset();
                    configurationDelegate(Nails.Configure().UnitOfWork.ConnectionBoundUnitOfWork(false));

                    currentReferenceResolver = new ReferenceResolver(Nails.ObjectFactory);
                    currentConfigurationName = testConfigurationName;
                }
                finally
                {
                    rwLock.DowngradeFromWriterLock(ref lockCookie);
                }
            }

            return currentReferenceResolver;
        }

        public void ReleaseConfiguration()
        {
            rwLock.ReleaseReaderLock();
        }

        public void SetConfigurationTimeout(int configurationTimeout)
        {
            ConfigurationTimeout = configurationTimeout;
        }

        public void SetInifiteConfigurationTimeout()
        {
            ConfigurationTimeout = Timeout.Infinite;
        }
    }
}
