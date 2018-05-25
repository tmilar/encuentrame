using NHibernate;

namespace NailsFramework.Persistence
{
    public interface INHibernateContext
    {
        ISession CurrentSession { get; }
    }
}