using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using NailsFramework.IoC;

namespace NailsFramework.UserInterface
{
    public class DependencyResolver : IDependencyResolver
    {
        public IObjectFactory ObjectFactory { get; private set; }

        public DependencyResolver(IObjectFactory objectFactory)
        {
            ObjectFactory = objectFactory;
        }
 
        public IDependencyScope BeginScope()
        {
            return new DependencyScope(ObjectFactory);
        }

        public object GetService(Type serviceType)
        {
            return ObjectFactory.GetObject(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ObjectFactory.GetObjects(serviceType).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}
