using System.Collections.Generic;

namespace Encuentrame.Model.Accounts
{
    public interface IRoleCommand
    {
        Role Get(int id);
        void Create(RoleCommand.CreateOrEditParameters roleParameters);
        IList<Role> List();
        void Edit(int id, RoleCommand.CreateOrEditParameters roleParameters);
        void Delete(int id);
    }
}