using System.Collections;

namespace NailsFramework.UnitOfWork.Session
{
    public class SingletonSessionContext : ISessionContext
    {
        private static readonly Hashtable SessionObjects = new Hashtable();

        #region ISessionContext Members

        /// <summary>
        ///   Gets the object from session.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <returns></returns>
        public T GetObject<T>(string key)
        {
            return !SessionObjects.ContainsKey(key) ? default(T) : (T) SessionObjects[key];
        }

        /// <summary>
        ///   Adds the object to session.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "val">The val.</param>
        public void SetObject(string key, object val)
        {
            SessionObjects[key] = val;
        }

        #endregion

        public void Dispose()
        {
            SessionObjects.Clear();
        }
    }
}