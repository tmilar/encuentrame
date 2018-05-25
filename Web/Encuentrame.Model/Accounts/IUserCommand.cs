using System.Collections;
using System.Collections.Generic;

namespace Encuentrame.Model.Accounts
{
    public interface IUserCommand
    {
        User Get(int id);
        void Create(UserCommand.CreateOrEditParameters userParameters);
        void NewRegister(UserCommand.CreateOrEditParameters userParameters);
        void EditRegister( UserCommand.CreateOrEditParameters userParameters);
        IList<User> List();
        IList<User> ListUsers();
        void Edit(int id, UserCommand.CreateOrEditParameters userParameters);
        void Delete(int id);
        void SetDevice(UserCommand.DeviceParameters deviceParameters);

        IList<User> GetUsersByIds(IEnumerable<int> ids);
    }
}