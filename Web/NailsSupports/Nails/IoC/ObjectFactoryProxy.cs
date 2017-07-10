using System;
using System.Collections;
using System.Collections.Generic;

namespace NailsFramework.IoC
{
    [Lemming]
    public class ObjectFactoryProxy : IObjectFactory
    {
        private static IObjectFactory ObjectFactory
        {
            get { return Nails.ObjectFactory; }
        }

        #region IObjectFactory Members

        public object GetObject(Type type)
        {
            return ObjectFactory.GetObject(type);
        }

        public IEnumerable GetObjects(Type type)
        {
            return ObjectFactory.GetObjects(type);
        }

        public object GetObject(string objectName)
        {
            return ObjectFactory.GetObject(objectName);
        }

        public T GetObject<T>() where T : class
        {
            return ObjectFactory.GetObject<T>();
        }

        public IEnumerable<T> GetObjects<T>() where T : class
        {
            return ObjectFactory.GetObjects<T>();
        }

        public T GetObject<T>(string objectName) where T : class
        {
            return ObjectFactory.GetObject<T>(objectName);
        }

        public object GetGenericObject<T>(string name)
        {
            return ObjectFactory.GetGenericObject<T>(name);
        }

        public object GetGenericObject<T1, T2>(string name)
        {
            return ObjectFactory.GetGenericObject<T1, T2>(name);
        }

        public object GetGenericObject<T1, T2, T3>(string name)
        {
            return ObjectFactory.GetGenericObject<T1, T2, T3>(name);
        }

        public object GetGenericObject<T1, T2, T3, T4>(string name)
        {
            return ObjectFactory.GetGenericObject<T1, T2, T3, T4>(name);
        }

        public object GetGenericObject<T1, T2, T3, T4, T5>(string name)
        {
            return ObjectFactory.GetGenericObject<T1, T2, T3, T4, T5>(name);
        }

        public object GetGenericObject(string name, Type typeParameter1, params Type[] othersTypeParameters)
        {
            return ObjectFactory.GetGenericObject(name, typeParameter1, othersTypeParameters);
        }

        public T Inject<T>(T instance) where T : class
        {
            return ObjectFactory.Inject(instance);
        }

        public object Inject(Type type, object instance)
        {
            return ObjectFactory.Inject(type, instance);
        }

        #endregion
    }
}