using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Security.Authorizations
{
    public interface IAuthorization
    {
        bool Validate(User user, Pass pass);
        bool Validate(string username, GroupsOfModulesEnum group, ModulesEnum module, ActionsEnum action);
    }
}