using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using NailsFramework.IoC;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Web.Support
{    
    public class AuthenticationDataProvider : IAuthenticationDataProvider
    {
        public IPrincipal GetCurrentUser
        {
            get
            {
                return HttpContext.Current.User;
            }
        }
    }
}