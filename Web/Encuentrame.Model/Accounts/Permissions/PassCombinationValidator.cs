using System.Linq;
using Encuentrame.Support;

namespace Encuentrame.Model.Accounts.Permissions
{
    public sealed class PassCombinationValidator
    {
        public static void Validate(GroupsOfModulesEnum group, ModulesEnum module, ActionsEnum action)
        {
            var moduleValid = false;
            var groupParents = module.GetAttributeOfEnumValue<GroupParentAttribute>().FirstOrDefault();
            moduleValid = groupParents == null || groupParents.Groups.Any(x => x == group);

            var actionValid = false;
            var moduleParents = action.GetAttributeOfEnumValue<ModuleParentAttribute>().FirstOrDefault();
            actionValid = moduleParents == null || moduleParents.Modules.Any(x => x == module);

            if (!actionValid || !moduleValid)
            {
                throw new PassCombinationInvalidException();
            }
        }
    }
}