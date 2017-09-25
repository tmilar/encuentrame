using Encuentrame.Model.Accounts;

namespace Encuentrame.Model.Supports.Interfaces
{
    public interface IAuthenticationProvider
    {
        bool ValidateUser(string username, string password, params RoleEnum[] validRoles);
        bool ChangePassword(string oldPassword, string newPassword);
        TokenApiSession GenerateApiTokenUser(string username);
        void RegenerateApiTokenUser(TokenApiSession token);
        bool ResetPassword(string username);
        BaseUser GetLoggedUser();
    }
}
