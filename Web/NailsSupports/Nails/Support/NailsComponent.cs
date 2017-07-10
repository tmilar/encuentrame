using System.Collections.Generic;
using NailsFramework.Config;

namespace NailsFramework.Support
{
    public abstract class NailsComponent
    {
        private readonly List<MissingConfiguration> missingConfigurations = new List<MissingConfiguration>();

        public IEnumerable<MissingConfiguration> MissingConfigurations
        {
            get { return missingConfigurations; }
        }

        public virtual void AddCustomConfiguration(INailsConfigurator configurator)
        {
        }

        public virtual void Initialize()
        {
        }

        protected void AddMissingConfiguration(MissingConfiguration missingConfiguration)
        {
            missingConfigurations.Add(missingConfiguration);
        }


        protected void AddMissingConfigurations(IEnumerable<MissingConfiguration> missingConfiguration)
        {
            missingConfigurations.AddRange(missingConfiguration);
        }
    }
}