using System.Collections.Generic;

namespace Encuentrame.Model.Accounts
{
    public interface IUserCommand
    {
        User Get(int id);
        void Create(UserCommand.CreateOrEditParameters userParameters);
        IList<User> List();
        void Edit(int id, UserCommand.CreateOrEditParameters userParameters);
        void Delete(int id);
    }
}