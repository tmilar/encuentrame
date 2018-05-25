using System;
using System.Web.Mvc;
using System.Web.Routing;
using NailsFramework.IoC;

namespace NailsFramework.UserInterface
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IObjectFactory objectFactory;

        public ControllerFactory(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? base.GetControllerInstance(requestContext, controllerType) //workaround: In this case, it will throw an exception. Not an actual return here.
                       : (IController) objectFactory.GetObject(controllerType);
        }
    }
}