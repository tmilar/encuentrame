using System;
using System.Collections;
using System.Collections.Generic;

namespace NailsFramework.IoC
{
    public class NullObjectFactory : IConfigurableObjectFactory
    {
        #region IConfigurableObjectFactory Members

        public object GetObject(Type type)
        {
            return null;
        }

        public IEnumerable GetObjects(Type type)
        {
            return new object[0];
        }

        public object GetObject(string objectName)
        {
            return null;
        }

        public T GetObject<T>() where T : class
        {
            return default(T);
        }

        public IEnumerable<T> GetObjects<T>() where T : class
        {
            return new T[0];
        }

        public T GetObject<T>(string objectName) where T : class
        {
            return null;
        }

        public object GetGenericObject<T>(string name)
        {
            return null;
        }

        public object GetGenericObject<T1, T2>(string name)
        {
            return null;
        }

        public object GetGenericObject<T1, T2, T3>(string name)
        {
            return null;
        }

        public object GetGenericObject<T1, T2, T3, T4>(string name)
        {
            return null;
        }

        public object GetGenericObject<T1, T2, T3, T4, T5>(string name)
        {
            return null;
        }

        public object GetGenericObject(string name, Type typeParameter1, params Type[] othersTypeParameters)
        {
            return null;
        }

        public T Inject<T>(T instance) where T : class
        {
            return instance;
        }

        public object Inject(Type type, object instance)
        {
            return null;
        }

        public void Configure(IEnumerable<Lemming> lemmings, IEnumerable<Injection> staticInjections)
        {
        }

        #endregion
    }
}