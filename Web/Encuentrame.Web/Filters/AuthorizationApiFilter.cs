using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

using Encuentrame.Support;
using Encuentrame.Web.Controllers.Apis;
using Encuentrame.Web.Helpers;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.Session;


namespace Encuentrame.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationApiFilter : AuthorizeAttribute
    {
        [Inject]
        public static IExecutionContext ExecutionContext { get; set; }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var typeController = actionContext.ControllerContext.Controller.GetType();
            var actionMethodInfo = actionContext.ActionDescriptor;
            var allowAnonymousController = Reflect.GetAttribute<AllowAnonymousAttribute>(typeController);
            var allowAnonymousAction = actionMethodInfo.GetCustomAttributes<AllowAnonymousAttribute>().FirstOrDefault();
            ExecutionContext.SetObject("isLogged", 0);
            if ( allowAnonymousAction == null)
            {
                var token = actionContext.Request.Headers.GetValues("token").FirstOrDefault();
                int userId = 0;
                int.TryParse(actionContext.Request.Headers.GetValues("user").FirstOrDefault(),out userId);

                if (!AuthorizationHelper.ValidateToken(userId,token))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                ExecutionContext.SetObject("isLogged", userId);
            }
          
        }
    }
}