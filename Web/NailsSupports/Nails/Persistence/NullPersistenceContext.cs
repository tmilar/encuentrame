using System;
using System.Reflection;

namespace NailsFramework.Persistence
{
    public class NullPersistenceContext : IPersistenceContext
    {
        #region IPersistenceContext Members

        public void OpenSession()
        {
        }

        public void CloseSession()
        {
        }

        public ITransactionalContext CreateTransactionalContext()
        {
            return new NullTransactionalContext();
        }

        public PropertyInfo GetIdPropertyOf<T>() where T : class
        {
            return null;
        }

        public bool IsSessionOpened
        {
            get { return false; }
        }

        #endregion
    }
}