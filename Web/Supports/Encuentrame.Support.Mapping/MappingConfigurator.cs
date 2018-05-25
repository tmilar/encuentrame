using FluentNHibernate;
using NailsFramework;
using NailsFramework.IoC;
using NHibernate.Cfg;

namespace Encuentrame.Support.Mappings
{
    public static class MappingConfigurator 
    {
        [Inject("MappingsPath")]
        public static string MappingsPath { private get; set; }

        public static void Configure( Configuration configuration, bool writeMappings)
        {
            var persistenceModel = new PersistenceModel();
            foreach (var assembly in Nails.Configuration.AssembliesToInspect)
            {
                persistenceModel.AddMappingsFromAssembly(assembly);
                persistenceModel.Conventions.AddAssembly(assembly);
                 
            }

            if (writeMappings)
                persistenceModel.WriteMappingsTo(MappingsPath);

            persistenceModel.Configure(configuration);
        }

        public static void Configure(Configuration configuration)
        {
            Configure( configuration,false);
        }
       
    }
}