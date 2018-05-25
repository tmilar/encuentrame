using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Security.Authentications
{
    public abstract class AuthenticationProvider : IAuthenticationProvider  
    {

        public abstract bool ValidateUser(string username, string password, params RoleEnum[] validRoles);

        public abstract TokenApiSession GenerateApiTokenUser(string  username);
        public abstract void RegenerateApiTokenUser(TokenApiSession token);
        public abstract void DeleteApiTokenUser(string username);

        public abstract bool ChangePassword(string oldPassword, string newPassword);

        public abstract bool ResetPassword(string username);
        public abstract BaseUser GetLoggedUser();
    }
}