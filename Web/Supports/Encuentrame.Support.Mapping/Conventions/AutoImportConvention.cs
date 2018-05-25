using FluentNHibernate.Conventions.Instances;

namespace Encuentrame.Support.Mappings.Conventions
{
    public class AutoImportConvention : FluentNHibernate.Conventions.IHibernateMappingConvention  
    {
        public void Apply(IHibernateMappingInstance instance)
        {
            instance.Not.AutoImport();
        }
    }
}