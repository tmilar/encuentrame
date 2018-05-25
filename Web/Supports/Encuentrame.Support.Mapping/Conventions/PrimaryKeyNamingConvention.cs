using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Encuentrame.Support.Mappings.Conventions
{
    public class PrimaryKeyNamingConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(ModelMapping.Id);
        }
    }
}
