using Encuentrame.Model.Accounts;

namespace Encuentrame.Model.Supports.Interfaces
{
    public interface IAuthenticationProvider
    {
        bool ValidateUser(string username, string password);
        bool ChangePassword(string oldPassword, string newPassword);
        bool ResetPassword(string username);
        BaseUser GetLoggedUser();
    }
}
