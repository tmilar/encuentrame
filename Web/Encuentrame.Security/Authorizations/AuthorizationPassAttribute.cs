using System;
using System.Linq;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Security.Authorizations
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method)]
    public class AuthorizationPassAttribute : Attribute
    {
        public AuthorizationPassAttribute(GroupsOfModulesEnum group, ModulesEnum module, ActionsEnum action)
        {
            PassCombinationValidator.Validate(group,module,action);
            this.Group = group;
            this.Module = module;
            this.Action = action;
        }

        public  GroupsOfModulesEnum Group { get; protected set; }
        public  ModulesEnum Module { get; protected set; }
        public  ActionsEnum Action { get; protected set; }
        
    }
}