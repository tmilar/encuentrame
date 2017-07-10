using System.Collections;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.UserInterface
{
    [Lemming(Singleton = false)]
    public class NailsControllerActionInvoker : ControllerActionInvoker
    {
        [Inject]
        public IObjectFactory ObjectFactory { private get; set; }
        [Inject]
        public IWorkContextProvider ContextProvider { private get; set; }

        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            Inject(filters.ActionFilters);
            Inject(filters.AuthorizationFilters);
            Inject(filters.ExceptionFilters);
            Inject(filters.ResultFilters);

            return filters;
        }
     
        private void Inject(IEnumerable objects)
        {
            foreach (var o in objects)
                ObjectFactory.Inject(o.GetType(), o);
        }
    }
}