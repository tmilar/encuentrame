using System.Collections.Generic;

namespace Encuentrame.Model.Activities
{
    public interface IActivityCommand
    {
        Activity Get(int id);
        void Create(ActivityCommand.CreateOrEditParameters eventParameters);

        IList<Activity> List();
        void Edit(int id, ActivityCommand.CreateOrEditParameters eventParameters);
        void Delete(int id);
    }
}