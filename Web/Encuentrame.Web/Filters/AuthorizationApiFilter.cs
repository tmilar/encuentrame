using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

using Encuentrame.Support;
using Encuentrame.Web.Helpers;


namespace Encuentrame.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationApiFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var typeController = actionContext.ControllerContext.Controller.GetType();
            var actionMethodInfo = actionContext.ActionDescriptor;
            var controller = Reflect.GetAttribute<AllowAnonymousAttribute>(typeController);
            var passAction = actionMethodInfo.GetCustomAttributes<AllowAnonymousAttribute>().FirstOrDefault();
            if ( passAction == null)
            {
                var token = actionContext.Request.Headers.GetValues("token").FirstOrDefault();
                int userId = 0;
                int.TryParse(actionContext.Request.Headers.GetValues("user").FirstOrDefault(),out userId);

                if (!AuthorizationHelper.ValidateToken(userId,token))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                
            }

        }
    }
}