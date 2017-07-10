using System;
using System.Collections.Generic;

namespace NailsFramework.UnitOfWork.Session
{
    public abstract class DisposableContext : IDisposable
    {
        private readonly List<string> keys = new List<string>();
        #region IExecutionContext Members

        public T GetObject<T>(string key)
        {
            var result = DoGetObject(key);
            return result != null ? (T)result : default(T);
        }

        public void SetObject(string key, object val)
        {
            if (keys.Contains(key)) DoRemoveObject(key);
            else keys.Add(key);

            DoSetObject(key, val);
        }

        public void RemoveObject(string key)
        {
            DoRemoveObject(key);
            keys.Remove(key);
        }

        protected abstract void DoSetObject(string key, object val);
        protected abstract object DoGetObject(string key);
        protected abstract void DoRemoveObject(string key);
        #endregion

        public void Dispose()
        {
            foreach (var key in keys)
                DoRemoveObject(key);
        }
    }
}