using System.Collections.Generic;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkCache
    {
        private readonly Dictionary<object, object> table = new Dictionary<object, object>();

        public virtual void AddItem(object key, object val)
        {
            table[key] = val;
        }

        public virtual T GetItem<T>(object key)
        {
            return (T) table[key];
        }
    }
}