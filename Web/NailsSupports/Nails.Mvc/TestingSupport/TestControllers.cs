using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;

namespace NailsFramework.UserInterface.TestingSupport
{
    public static class TestControllers
    {
        [Inject]
        public static IVirtualMethodsProxyFactory ProxyFactory { private get; set; }
        [Inject]
        public static IObjectFactory ObjectFactory { private get; set; }

        public static TController Get<TController>() where TController : NailsController, new()
        {
            var aspect = new Aspect(new UnitOfWorkBehavior(), new BehaviorCondition(IsAction, new List<string>()));

            var controller = ProxyFactory.Create<TController>(aspect);
            ObjectFactory.Inject(controller);
            return controller;
        }

#if MVC4
        public static TApiController GetApiController<TApiController>() where TApiController : NailsApiController, new()
        {
            var aspect = new Aspect(new UnitOfWorkBehavior(), new BehaviorCondition(IsApiAction, new List<string>()));

            var apiController = ProxyFactory.Create<TApiController>(aspect);
            ObjectFactory.Inject(apiController);
            return apiController;
        }

        private static bool IsApiAction(MethodBase method)
        {
            var info = method as MethodInfo;
            var isAction = info != null && info.IsPublic;
            return isAction;
        }
#endif
#if MVC5
        public static TApiController GetApiController<TApiController>() where TApiController : NailsApiController, new()
        {
            var aspect = new Aspect(new UnitOfWorkBehavior(), new BehaviorCondition(IsApiAction, new List<string>()));

            var apiController = ProxyFactory.Create<TApiController>(aspect);
            ObjectFactory.Inject(apiController);
            return apiController;
        }

        private static bool IsApiAction(MethodBase method)
        {
            var info = method as MethodInfo;
            var isAction = info != null && info.IsPublic;
            return isAction;
        }
#endif
        private static bool IsAction(MethodBase method)
        {
            var info = method as MethodInfo;
            var isAction = info != null && info.IsPublic && typeof (ActionResult).IsAssignableFrom(info.ReturnType);
            return isAction;
        }
    }
}