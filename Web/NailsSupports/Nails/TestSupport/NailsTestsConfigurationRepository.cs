using System;
using System.Collections.Generic;
using NailsFramework.Config;

namespace NailsFramework.TestSupport
{
    /// <summary>
    /// Repository of nails configuration that will be globally available during the test runtime so that your test classes can register and use the 
    /// appropriate configuration. Please make sure you correctly register your tests' configurations by calling the <see cref="Set"/> method.
    /// </summary>
    public sealed class NailsTestsConfigurationRepository
    {
        public static readonly NailsTestsConfigurationRepository Instance = new NailsTestsConfigurationRepository();

        private IDictionary<string, Action<INailsConfigurator>> nailsConfigurationsMap;

        private NailsTestsConfigurationRepository()
        {
            nailsConfigurationsMap = new Dictionary<string, Action<INailsConfigurator>>();
        }

        public void Set(string configurationName, Action<INailsConfigurator> nailsConfiguration)
        {
            if (nailsConfigurationsMap.ContainsKey(configurationName))
                throw new DuplicatedNailsTestsConfigurationException(configurationName);

            nailsConfigurationsMap[configurationName] = nailsConfiguration;
        }

        public Action<INailsConfigurator> Get(string configurationName)
        {
            if (!nailsConfigurationsMap.ContainsKey(configurationName))
                throw new MissingNailsTestsConfigurationException(configurationName);

            return nailsConfigurationsMap[configurationName];
        }

        public void Reset(string configurationName)
        {
            if (!nailsConfigurationsMap.ContainsKey(configurationName))
                throw new MissingNailsTestsConfigurationException(configurationName);

            nailsConfigurationsMap.Remove(configurationName);
        }

        public void Clear()
        {
            nailsConfigurationsMap.Clear();
        }
    }
}