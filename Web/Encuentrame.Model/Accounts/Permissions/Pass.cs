using Encuentrame.Support;

namespace Encuentrame.Model.Accounts.Permissions
{
    public class Pass:IIdentifiable
    {
        public Pass()
        {
            
        }
        public Pass(GroupsOfModulesEnum group, ModulesEnum module, ActionsEnum action)
        {
            Group = group;
            Module = module;
            Action = action;
            PassCombinationValidator.Validate(Group, Module, Action);
        }

        public virtual bool IsValid()
        {
            try
            {
                PassCombinationValidator.Validate(Group, Module, Action);
                return true;
            }
            catch (PassCombinationInvalidException ex)
            {
                return false;
            }
            
        }

        public virtual int Id { get; protected set; }
        public virtual GroupsOfModulesEnum Group { get; protected set; }
        public virtual ModulesEnum Module { get; protected set; }
        public virtual ActionsEnum Action { get; protected set; }
       
    }
}