using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Encuentrame.Model.Accounts;
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

        public static bool Validate(RoleEnum role)
        {
            var roles = new RoleEnum[] { role };
            return Validate(roles);
        }
        public static bool Validate(RoleEnum[] roles)
        {
            return Authorization.Validate(HttpContext.Current.User.Identity.Name, roles);
        }

        public static bool Validate<T>() where T: class 
        {
            var pass= Reflect.GetAttribute<AuthorizationPassAttribute>(typeof(T));
            return pass != null ? Validate(pass.Roles) : true;
        }

        public static bool ValidateToken(int userId,string token)
        {
            return Authorization.ValidateToken(userId, token);
            
        }
    }
   
}