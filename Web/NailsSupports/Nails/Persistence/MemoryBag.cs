using System.Linq;
using MemoDb;
using NailsFramework.IoC;

namespace NailsFramework.Persistence
{
    public class MemoryBag<T> : Bag<T> where T : class
    {
        [Inject]
        public MemoryContext Context { private get; set; }
        private MemoSession Session { get { return Context.CurrentSession; } }
        public override T this[object id]
        {
            get { return Session.GetById<T>(id); }
        }

        public override void Put(T item)
        {
            Session.Insert(item);
        }

        protected override IQueryable<T> GetQueryable()
        {
            return Session.Query<T>();
        }

        public override void Remove(T o)
        {
            Session.Delete(o);
        }
    }
}