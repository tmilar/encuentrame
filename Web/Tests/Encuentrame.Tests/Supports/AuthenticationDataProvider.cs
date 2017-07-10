using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Tests.Supports
{
    public class AuthenticationDataProvider : IAuthenticationDataProvider
    {
        public IPrincipal GetCurrentUser
        {
            get
            {
                return null;
            }
        }
    }
}
