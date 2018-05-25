using System.Collections.Generic;
using System.Linq;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Null bag implementation
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class NullBag<T> : Bag<T> where T : class
    {
        public override T this[object id]
        {
            get { return null; }
        }

        protected override IQueryable<T> GetQueryable()
        {
            return new List<T>().AsQueryable();
        }

        public override void Put(T o)
        {
        }

        public override void Remove(T o)
        {
        }
    }
}