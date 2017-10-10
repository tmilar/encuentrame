using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Events;
using Encuentrame.Model.Supports;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.Activities
{
    [Lemming]
    public class ActivityCommand : BaseCommand, IActivityCommand
    {
        [Inject]
        public IBag<Activity> Activities { get; set; }

        [Inject]
        public IBag<Event> Events { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }


        public Activity Get(int id)
        {
            return Activities[id];
        }

        public IList<Activity> GetActivities(int userId)
        {
            var user = Users[userId];
            return Activities.Where(x=>x.User==user).ToList();
        }

        public IList<Activity> List()
        {
            return Activities.ToList();
        }
        public void Create(CreateOrEditParameters eventParameters)
        {
            var activity = new Activity();
            UpdateWith(activity, eventParameters);

            Activities.Put(activity);
        }
        public void Edit(int id, CreateOrEditParameters eventParameters)
        {
            var activity = Activities[id];
            UpdateWith(activity, eventParameters);

            Activities.Put(activity);
        }

        private void UpdateWith(Activity activity, CreateOrEditParameters eventParameters)
        {
            activity.User = Users[eventParameters.UserId];
            activity.Name = eventParameters.Name;
            activity.Latitude = eventParameters.Latitude;
            activity.Longitude = eventParameters.Longitude;
            activity.BeginDateTime = eventParameters.BeginDateTime;
            activity.EndDateTime = eventParameters.EndDateTime;
            activity.Event = eventParameters.EventId.HasValue? Events[eventParameters.EventId]:null;

        }
        public void Delete(int id)
        {
            var activity = Activities[id];
            Activities.Remove(activity);
        }

        public class CreateOrEditParameters
        {
            public string Name { get; set; }

            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }

            public DateTime BeginDateTime { get; set; }
            public DateTime EndDateTime { get; set; }

            public int? EventId { get; set; }
            public int UserId { get; set; }
        }
    }
}