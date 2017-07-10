using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NailsFramework.IoC;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class AuthorizationHelper
    {
        [Inject]
        public static IAuthorization Authorization { get; set; }
        public static bool Validate(AuthorizationPassAttribute pass)
        {
            return Validate(pass.Group,pass.Module,pass.Action);
        }
        public static bool Validate(GroupsOfModulesEnum group, ModulesEnum module, ActionsEnum action)
        {
            return Authorization.Validate(HttpContext.Current.User.Identity.Name, group, module, action);
        }

        public static bool Validate<T>() where T: class 
        {
            var pass= Reflect.GetAttribute<AuthorizationPassAttribute>(typeof(T));
            return pass != null ? Validate(pass) : true;
        }

    }
}