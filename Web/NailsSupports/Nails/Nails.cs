using System;
using NailsFramework.Config;
using NailsFramework.IoC;

namespace NailsFramework
{
    public static class Nails
    {
        private static NailsConfiguration configuration;

        static Nails()
        {
            configuration = new NailsConfiguration();
        }

        public static INailsConfiguration Configuration
        {
            get { return configuration; }
        }

        public static IConfigurationStatus Status
        {
            get { return configuration; }
        }

        public static IObjectFactory ObjectFactory
        {
            get { return configuration.ObjectFactory; }
        }

        public static void Reset(bool disposeLemmingsOnReset = true)
        {
            if ( disposeLemmingsOnReset && ObjectFactory != null)
                foreach (var disposable in ObjectFactory.GetObjects<IDisposable>())
                    disposable.Dispose();

            configuration = new NailsConfiguration();
        }

        public static INailsConfigurator Configure()
        {
            if (configuration.Initialized)
                throw new InvalidOperationException(
                    "Cannot configure Nails once it was already initialized. If you want to reconfigure Nails, call Reset() method first.");
            return configuration;
        }

        public static void Initialize(bool configureDefaults=true)
        {
            Configure().Initialize(configureDefaults);
        }
    }
}