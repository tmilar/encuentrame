using System.Collections;
using System.Collections.Generic;

namespace NailsFramework.IoC
{
    public class LemmingsCollection<T> : IEnumerable<T> where T : class
    {
        [Inject]
        public IObjectFactory ObjectFactory { private get; set; }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ObjectFactory.GetObjects<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}