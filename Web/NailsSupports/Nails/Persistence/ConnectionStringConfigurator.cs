using System.Collections.Generic;
using System.Configuration;
using NailsFramework.Config;

namespace NailsFramework.Persistence
{
    public class ConnectionStringConfigurator
    {
        private readonly IConnectionStringProvider provider;

        public ConnectionStringConfigurator(IConnectionStringProvider provider)
        {
            this.provider = provider;
        }
        private readonly List<MissingConfiguration> missingConfigurations = new List<MissingConfiguration>();

        public IEnumerable<MissingConfiguration> MissingConfigurations
        {
            get { return missingConfigurations; }
        }

        private bool ValidateConnectionString()
        {
            if (string.IsNullOrWhiteSpace(provider.ConnectionStringName))
            {
                var connStringValue = ConnectionString;

                if (string.IsNullOrEmpty(connStringValue))
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format("Connection string is not configured. Ensure one of the settings {0} and {1} is configured with your connection string in the <appSettings> section of your configuration file. Also, if you provided the EntityFramework.ConnectionStringName with your connection string name, ensure that connection string exists in the <connectionStrings> section of your configuration file.",provider.ConnectionStringKey,provider.ConnectionStringNameKey)));
                    return false;
                }
            }
            else
            {
                var connString = ConfigurationManager.ConnectionStrings[provider.ConnectionStringName];
                if (connString == null)
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format(
                                "Connection string {0} is missing in your configuration file. Add it to the <connectionStrings> section of your configuration file.",
                                provider.ConnectionStringName)));
                    return false;
                }

                if (string.IsNullOrWhiteSpace(connString.ConnectionString))
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format(
                                "You need to configure the connection string. In your configuration file, inside the <connectionStrings> section you will find an empty entry called {0}. Put there your connection string.",
                                provider.ConnectionStringName)));
                    return false;
                }
            }
            return true;
        }
        public bool Configure()
        {
            if (!ValidateConnectionString())
                return false;

            if (!string.IsNullOrWhiteSpace(provider.ConnectionString) && !string.IsNullOrWhiteSpace(provider.ConnectionStringName))
                throw new NailsConfigurationException(
                    "Both ConnectionString and ConnectionStringName are configured. Choose only one.");

            ConnectionString = !string.IsNullOrWhiteSpace(provider.ConnectionString)
                                   ? provider.ConnectionString
                                   : ConfigurationManager.ConnectionStrings[provider.ConnectionStringName].ConnectionString;

            return true;
        }

        public string ConnectionString { get; private set; }
    }
}