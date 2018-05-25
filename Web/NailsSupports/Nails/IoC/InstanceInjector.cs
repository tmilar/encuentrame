using System;
using System.Collections.Generic;

namespace NailsFramework.IoC
{
    public class InstanceInjector
    {
        private readonly Dictionary<Type, TypeInjector> injectors = new Dictionary<Type, TypeInjector>();
        private readonly IObjectFactory objectFactory;
        private static readonly object SyncObject = new object();

        public InstanceInjector(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public void Inject(Type type, object instance)
        {
            if (instance == null)
                return;

            TypeInjector injector;

            if (!injectors.TryGetValue(type, out injector))
            {
                lock (SyncObject)
                {
                    if (!injectors.TryGetValue(type, out injector))
                    {
                        injector = new TypeInjector(type, objectFactory);
                        injectors.Add(type, injector);
                    }
                }
            }

            injector.Inject(instance);
        }
    }
}