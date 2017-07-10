using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Encuentrame.Support;

using System.Web.Mvc;
using NailsFramework.Support;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models;

namespace Encuentrame.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var typeController = filterContext.Controller.GetType();
                var actionMethodInfo = filterContext.ActionDescriptor;
                var passController = Reflect.GetAttribute<AuthorizationPassAttribute>(typeController);
                var passAction = actionMethodInfo.Attribute<AuthorizationPassAttribute>();

                var passControllerIsValid = true;
                var passActionIsValid = true;

                if (passController != null)
                {
                    passControllerIsValid = AuthorizationHelper.Validate(passController);
                }

                if (passAction != null)
                {
                    passActionIsValid = AuthorizationHelper.Validate(passAction);
                }

                if (!(passActionIsValid && passControllerIsValid))
                {
                    HandleUnauthorizedInfo model = null;
                    if (!passControllerIsValid)
                    {
                        model = new HandleUnauthorizedInfo(passController);
                    }
                    else
                    {
                        model = new HandleUnauthorizedInfo(passAction);
                    }

                    filterContext.Result = new ViewResult
                    {
                        ViewName = "Unauthorized",
                        TempData = filterContext.Controller.TempData,
                        ViewData = new ViewDataDictionary<HandleUnauthorizedInfo>(model),
                    };

                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 500;
                }
            }
        }


    }
}