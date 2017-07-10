using FluentNHibernate.Mapping;

namespace Encuentrame.Support.Mappings
{
    public abstract class BaseSubclassMapping<T> : SubclassMap<T> where T : IIdentifiable
    {
        protected BaseSubclassMapping()
        {
            Table(ModelMapping.TableNameFor<T>());
        }
    }
}
