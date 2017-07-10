using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Security.Authorizations
{
    [Lemming]
    public class Authorization : IAuthorization
    {
        [Inject]
        public IBag<User> Users { get; set; }

        public bool Validate(User user, Pass pass)
        {
            return user.Role.Passes.Contains(pass);
        }

        public bool Validate(string username, GroupsOfModulesEnum group,ModulesEnum module, ActionsEnum action)
        {
            PassCombinationValidator.Validate(group, module, action);
            var user = Users.FirstOrDefault(x => x.Username == username && x.DeletedKey == null  && x.Role!=null);
            if(user==null)
            {
                return false;
            }

            return user.Role.Passes.Any(x => x.Group == group && x.Module == module && x.Action == action);
        }
    }
}
