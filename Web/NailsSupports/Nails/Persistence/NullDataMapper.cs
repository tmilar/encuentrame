using System;
using NailsFramework.Config;

namespace NailsFramework.Persistence
{
    public class NullDataMapper : DataMapper
    {
        public override Type BagType
        {
            get { return typeof (NullBag<>); }
        }

        public override Type PersistenceContextType
        {
            get { return typeof (NullPersistenceContext); }
        }

        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
        }

        public override void ConfigureBags(LemmingConfigurator baglemming)
        {
        }

        public override void ConfigurePersistenceContext(LemmingConfigurator persistenceContextLemming)
        {
        }
    }
}