using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NailsFramework.Persistence
{
    public abstract class Bag<T> : IBag<T> where T : class
    {
        #region IBag<T> Members

        public abstract T this[object id] { get; }
        public abstract void Put(T o);
        public abstract void Remove(T o);

        public IEnumerator<T> GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression
        {
            get { return GetQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return typeof (T); }
        }

        public IQueryProvider Provider
        {
            get { return GetQueryable().Provider; }
        }

        #endregion

        protected abstract IQueryable<T> GetQueryable();
    }
}