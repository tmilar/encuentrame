using System;
using NHibernate.Cfg;

namespace NailsFramework.Persistence
{
    public class NHibernate : DataMapper
    {
        private Action<Configuration> configure = c=>{};

        public override Type BagType
        {
            get { return typeof (NHibernateBag<>); }
        }

        public override Type PersistenceContextType
        {
            get { return typeof (NHibernateContext); }
        }

        public string ConfigurationFile { get; set; }

        public override void Initialize()
        {
            var context = Nails.ObjectFactory.GetObject<NHibernateContext>();
            
            if (!string.IsNullOrWhiteSpace(ConfigurationFile))
                context.ConfigurationFile = ConfigurationFile;

            context.Initialize(configure);
            AddMissingConfigurations(context.MissingConfigurations);
        }

        public NHibernate Configure(Action<Configuration> configure)
        {
            this.configure = configure;
            return this;
        }
    }
}