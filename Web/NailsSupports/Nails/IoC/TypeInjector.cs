using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NailsFramework.IoC
{
    public class TypeInjector
    {
        private readonly Lemming lemming;
        private readonly IObjectFactory objectFactory;

        private readonly Dictionary<PropertyInfo, PropertySetter> propertiesCache =
            new Dictionary<PropertyInfo, PropertySetter>();

        public TypeInjector(Type type, IObjectFactory objectFactory)
        {
            lemming = Lemming.From(type);
            CreatePropertiesCache();

            this.objectFactory = objectFactory;
        }

        private void CreatePropertiesCache()
        {
            foreach (var property in lemming.Injections.Select(x => x.Property))
            {
                var setter = PropertySetter.From(property);
                propertiesCache.Add(property, setter);
            }
        }

        public void Inject(object instance)
        {
            foreach (var injection in lemming.Injections)
                injection.Accept(new Injector(instance, propertiesCache, objectFactory));
        }

        #region Nested type: Injector

        private class Injector : IInjector
        {
            private readonly object instance;
            private readonly IObjectFactory objectFactory;
            private readonly Dictionary<PropertyInfo, PropertySetter> propertiesCache;

            public Injector(object instance, Dictionary<PropertyInfo, PropertySetter> propertiesCache,
                            IObjectFactory objectFactory)
            {
                this.instance = instance;
                this.objectFactory = objectFactory;
                this.propertiesCache = propertiesCache;
            }

            #region IInjector Members

            public void Inject(ValueInjection injection)
            {
                propertiesCache[injection.Property].SetValue(instance, injection.Value);
            }

            public void Inject(ReferenceInjection injection)
            {
                var value = new ReferenceResolver(objectFactory).GetReferenceFor(injection);
                propertiesCache[injection.Property].SetValue(instance, value);
            }

            #endregion
        }

        #endregion
    }
}