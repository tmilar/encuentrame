using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts;

namespace Encuentrame.Security.Authorizations
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method)]
    public class AuthorizationPassAttribute : Attribute
    {
        public AuthorizationPassAttribute(RoleEnum[] roles)
        {
            this.Roles = roles;
        }

      
        public RoleEnum[] Roles { get; protected set; }
        
    }
}