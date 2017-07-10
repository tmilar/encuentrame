using System;
using System.Collections;
using System.Collections.Generic;

namespace NailsFramework.IoC
{
    /// <summary>
    ///   Object Factory.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        ///   Finds the implementation of the object. Usually invoked using an
        ///   interface as parameter.
        /// </summary>
        /// <param name = "type">Type to create.</param>
        /// <returns>The newly created object.</returns>
        object GetObject(Type type);

        /// <summary>
        ///   Finds the implementations of the object. Usually invoked using an
        ///   interface as parameter.
        /// </summary>
        /// <param name = "type">Type to create.</param>
        /// <returns>The newly created object.</returns>
        IEnumerable GetObjects(Type type);

        /// <summary>
        ///   Finds the implementation of the object.
        /// </summary>
        /// <param name = "objectName">The identifier of the object.</param>
        /// <returns>The newly created object.</returns>
        object GetObject(string objectName);

        /// <summary>
        ///   Generic factory method that finds the implementation of the object.
        ///   Usually invoked using an interface as parameter.
        /// </summary>
        /// <typeparam name = "T">The type.</typeparam>
        /// <returns>The newly created object.</returns>
        T GetObject<T>() where T : class;

        /// <summary>
        ///   Generic factory method that finds the implementations of the object.
        ///   Usually invoked using an interface as parameter.
        /// </summary>
        /// <typeparam name = "T">The type.</typeparam>
        /// <returns>The newly created object.</returns>
        IEnumerable<T> GetObjects<T>() where T : class;

        T GetObject<T>(string objectName) where T : class;

        object GetGenericObject<T>(string name);
        object GetGenericObject<T1, T2>(string name);
        object GetGenericObject<T1, T2, T3>(string name);
        object GetGenericObject<T1, T2, T3, T4>(string name);
        object GetGenericObject<T1, T2, T3, T4, T5>(string name);
        object GetGenericObject(string name, Type typeParameter1, params Type[] othersTypeParameters);

        T Inject<T>(T instance) where T : class;
        object Inject(Type type, object instance);
    }
}