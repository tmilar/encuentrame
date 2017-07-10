using System;
using System.Collections;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.Tests.Support
{
    public class TestExecutionContext : IExecutionContext
    {
        private readonly Hashtable hashtable = new Hashtable();

        #region IExecutionContext Members

        public T GetObject<T>(string key)
        {
            return (T) hashtable[key];
        }

        public void SetObject(string key, object val)
        {
            if (hashtable.Contains(key))
                RemoveObject(key);
            hashtable.Add(key, val);
        }

        public void RemoveObject(string key)
        {
            hashtable.Remove(key);
        }

        #endregion

        public void Dispose()
        {
            hashtable.Clear();
        }
    }
}