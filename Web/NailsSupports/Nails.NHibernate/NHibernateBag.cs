using System;
using System.Linq;
using NailsFramework.IoC;
using NHibernate;
using NHibernate.Linq;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   A NHibernate based bag implementation.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class NHibernateBag<T> : Bag<T> where T : class
    {
        [Inject]
        public INHibernateContext NHibernateContext { private get; set; }

        protected virtual ISession Session
        {
            get { return NHibernateContext.CurrentSession; }
        }

        public override T this[object id]
        {
            get { return Session.Load<T>(id); }
        }

        /// <summary>
        ///   Returns all the objects in the bag.
        /// </summary>
        /// <returns>A Generic List of all the objects.</returns>
        protected override IQueryable<T> GetQueryable()
        {
            return Session.Query<T>();
        }

        public override void Put(T o)
        {
            Session.Save(o);
        }

        public override void Remove(T o)
        {
            Session.Delete(o);
        }
    }
}