using System;
using System.Collections;
using System.Collections.Generic;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace NailsFramework.IoC
{
    /// <summary>
    ///   Object factory for Spring.NET
    /// </summary>
    public class SpringObjectFactory : BaseObjectFactory
    {
        private readonly GenericApplicationContext applicationContext;

        private readonly IObjectDefinitionRegistry objectDefinitionRegistry;

        public SpringObjectFactory()
        {
            applicationContext = new GenericApplicationContext();

            ContextRegistry.Clear();
            ContextRegistry.RegisterContext(applicationContext);

            objectDefinitionRegistry = (IObjectDefinitionRegistry) applicationContext.ObjectFactory;
        }

        /// <summary>
        ///   Finds the implementation of the object. Usually invoked using an
        ///   interface as parameter.
        /// </summary>
        /// <param name = "type">Type to create.</param>
        /// <returns>The newly created object.</returns>
        protected override object FindObjectByType(Type type)
        {
            var dictionary = applicationContext.GetObjectsOfType(type);
            var enumerator = dictionary.Values.GetEnumerator();
            var obj = enumerator.MoveNext() ? enumerator.Current : null;

            return obj;
        }

        public override void Configure(IEnumerable<Lemming> lemmings, IEnumerable<Injection> staticInjections)
        {
            base.Configure(lemmings, staticInjections);
            applicationContext.Refresh();
        }

        /// <summary>
        ///   Finds the implementation of the object. Usually invoked using an
        ///   interface as parameter.
        /// </summary>
        /// <param name = "type">Type to create.</param>
        /// <returns>The newly created object.</returns>
        protected override IEnumerable FindObjectsByType(Type type)
        {
            var dictionary = applicationContext.GetObjectsOfType(type);
            return dictionary.Values;
        }

        /// <summary>
        ///   Finds the implementation of the object.
        /// </summary>
        /// <param name = "objectName">The identifier of the object.</param>
        /// <returns>The newly created object.</returns>
        protected override object FindObjectByName(string objectName)
        {
            return applicationContext.ContainsObject(objectName) ? applicationContext.GetObject(objectName) : null;
        }

        /// <summary>
        ///   Adds the lemming to the current context
        /// </summary>
        /// <param name = "lemming">The lemming to add</param>
        protected override void AddToContext(Lemming lemming)
        {
            var builder = ObjectDefinitionBuilder.RootObjectDefinition(new DefaultObjectDefinitionFactory(),
                                                                       lemming.ConcreteType);

            builder.SetAutowireMode(AutoWiringMode.ByName);
            builder.SetSingleton(lemming.Singleton);

            foreach (var injection in lemming.Injections)
                injection.Accept(new SpringInjector(injection.Property.Name, builder));

            objectDefinitionRegistry.RegisterObjectDefinition(lemming.UniqueName, builder.ObjectDefinition);
        }
    }
}