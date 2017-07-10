using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using NailsFramework.IoC;
using NailsFramework.Support;

namespace NailsFramework.UserInterface
{
#warning Review implmentation, try to use a nested scope rather than the object factory itself
    public class DependencyScope : IDependencyScope
    {
        public IObjectFactory ObjectFactory { get; private set; }

        public DependencyScope(IObjectFactory objectFactory)
        {
            ObjectFactory = objectFactory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.Is<System.Web.Http.Controllers.IHttpActionInvoker>() || serviceType.Is<System.Web.Http.Filters.IFilterProvider>())
                serviceType.ToString();
            return ObjectFactory.GetObject(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType.Is<System.Web.Http.Controllers.IHttpActionInvoker>() || serviceType.Is<System.Web.Http.Filters.IFilterProvider>())
                serviceType.ToString();
            return ObjectFactory.GetObjects(serviceType).Cast<object>();
        }
        public void Dispose()
        {
        }
    }
}
