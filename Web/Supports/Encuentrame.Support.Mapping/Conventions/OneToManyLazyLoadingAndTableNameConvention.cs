using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace EPA.Support.Mappings.Conventions
{
    public class OneToManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Table(instance.EntityType.Name + "_" + instance.Member.Name);
            
            instance.LazyLoad();
        }
    }
}
