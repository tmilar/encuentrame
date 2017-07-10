using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Security.Authentications
{
    public abstract class AuthenticationProvider : IAuthenticationProvider  
    {

        public abstract bool ValidateUser(string username, string password);
        public abstract bool ChangePassword(string oldPassword, string newPassword);

        public abstract bool ResetPassword(string username);
        public abstract BaseUser GetLoggedUser();
    }
}