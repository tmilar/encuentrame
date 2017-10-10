using System.Collections.Generic;

namespace Encuentrame.Model.Activities
{
    public interface IActivityCommand
    {
        Activity Get(int id);
        IList<Activity> GetActivities(int userId);
        void Create(ActivityCommand.CreateOrEditParameters eventParameters);

        IList<Activity> List();
        void Edit(int id, ActivityCommand.CreateOrEditParameters eventParameters);
        void Delete(int id);
    }
}