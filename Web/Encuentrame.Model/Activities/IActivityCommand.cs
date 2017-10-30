using System.Collections.Generic;

namespace Encuentrame.Model.Activities
{
    public interface IActivityCommand
    {
        Activity Get(int id);
        IList<Activity> GetAllByUser(int userId);
        IList<Activity> GetActivesByUser(int userId);
        void Create(ActivityCommand.CreateOrEditParameters eventParameters);

        IList<Activity> List();
        void Edit(int id, ActivityCommand.CreateOrEditParameters eventParameters);
        void Delete(int id);
    }
}