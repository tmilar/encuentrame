using System;
using NailsFramework.Config;
using NailsFramework.Support;

namespace NailsFramework.Persistence
{
    public abstract class DataMapper : NailsComponent
    {
        public abstract Type BagType { get; }
        public abstract Type PersistenceContextType { get; }

        public virtual void ConfigureBags(LemmingConfigurator baglemming)
        {
        }

        public virtual void ConfigurePersistenceContext(LemmingConfigurator persistenceContextLemming)
        {
        }
    }
}