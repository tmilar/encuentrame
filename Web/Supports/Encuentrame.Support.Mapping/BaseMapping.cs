using FluentNHibernate.Mapping;

namespace Encuentrame.Support.Mappings
{
    public abstract class BaseMapping<T> : ClassMap<T> where T : IIdentifiable
    {
        protected BaseMapping()
        {
            ConfigureId();
            Table(ModelMapping.TableNameFor<T>());
        }

        protected virtual void ConfigureId()
        {
            Id(x => x.Id); //.GeneratedBy.HiLo("HiLoIds", "HiLo", "1000");
        }
       
    }
}