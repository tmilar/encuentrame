using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Security.Authorizations;

namespace Encuentrame.Web.Models
{
    public class HandleUnauthorizedInfo
    {
        public HandleUnauthorizedInfo(AuthorizationPassAttribute pass)
        {
            Group = pass.Group;
            Module = pass.Module;
            Action = pass.Action;
        }
        public GroupsOfModulesEnum Group { get; protected set; }
        public ModulesEnum Module { get; protected set; }
        public ActionsEnum Action { get; protected set; }
    }
}