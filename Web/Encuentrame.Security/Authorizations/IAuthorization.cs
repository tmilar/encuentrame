using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Security.Authorizations
{
    public interface IAuthorization
    {
        
        bool Validate(string username, RoleEnum[] roles);
        bool ValidateToken(int userId,string token);
    }
}