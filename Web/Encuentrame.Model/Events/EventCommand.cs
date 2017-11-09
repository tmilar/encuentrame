using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Transform;

namespace Encuentrame.Model.Events
{
    [Lemming]
    public class EventCommand : BaseCommand, IEventCommand
    {
        [Inject]
        public IBag<Event> Events { get; set; }


        [Inject]
        public IBag<AreYouOkEvent> AreYouOkEvents { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        [Inject]
        public AreYouOkCommand AreYouOkCommand { get; set; }

        public Event Get(int id)
        {
            return Events[id];
        }
        public IList<Event> List()
        {
            return Events.Where(x => x.DeletedKey == null).ToList();
        }
        public void Create(CreateOrEditParameters eventParameters)
        {
            var eventt = new Event();
            UpdateWith(eventt, eventParameters);
            if (SystemDateTime.Now.Between(eventt.BeginDateTime, eventt.EndDateTime))
            {
                eventt.Status = EventStatusEnum.InProgress;
            }
            else if (SystemDateTime.Now < eventt.BeginDateTime)
            {
                eventt.Status = EventStatusEnum.Pending;
            }
            Events.Put(eventt);
        }
        public void Edit(int id, CreateOrEditParameters eventParameters)
        {
            var eventt = Events[id];
            UpdateWith(eventt, eventParameters);

            Events.Put(eventt);
        }

     
        private void UpdateWith(Event eventt, CreateOrEditParameters eventParameters)
        {
            eventt.Name = eventParameters.Name;
            eventt.Latitude = eventParameters.Latitude;
            eventt.Longitude = eventParameters.Longitude;
            eventt.BeginDateTime = eventParameters.BeginDateTime;
            eventt.EndDateTime = eventParameters.EndDateTime;
            eventt.Address=new Address()
            {
                City = eventParameters.City,
                FloorAndDepartament = eventParameters.FloorAndDepartament,
                Number = eventParameters.Number,
                Province = eventParameters.Province,
                Street = eventParameters.Street,
                Zip = eventParameters.Zip
            };
            eventt.Organizer = Users[eventParameters.OrganizerId];

            if (eventParameters.UsersIds != null)
            {
                EditListFromIds(eventParameters.UsersIds, eventt.Users, i => Users[i]);
            }
            
        }
        public void Delete(int id)
        {
            var eventt=Events[id];
            eventt.DeletedKey = SystemDateTime.Now;
        }

        public void DeclareEmergency(int id)
        {
            var eventt = Events[id];
            if (eventt.Status.In(EventStatusEnum.InProgress, EventStatusEnum.InEmergency))
            {
                eventt.Status = EventStatusEnum.InEmergency;
                eventt.EmergencyDateTime = SystemDateTime.Now;
                AreYouOkCommand.AskFromEvent(eventt);
            }
            else
            {
                throw new DeclareEmergencyException();
            }

        }

        public void CancelEmergency(int id)
        {
            var eventt = Events[id];
            if (eventt.Status.In( EventStatusEnum.InEmergency))
            {
                eventt.Status = EventStatusEnum.InProgress;
                eventt.EmergencyDateTime = null;
            }
            else
            {
                throw new DeclareEmergencyException();
            }
        }

        public void BeginEvent(int id)
        {
            var eventt = Events[id];
            eventt.Status=EventStatusEnum.InProgress;
            eventt.BeginDateTime = SystemDateTime.Now;
            //TODO: hacer las notifications
        }

        public void FinalizeEvent(int id)
        {
            var eventt = Events[id];
            eventt.Status = EventStatusEnum.Completed;
            eventt.EndDateTime = SystemDateTime.Now;
            //TODO: hacer las notifications
        }

        public void StartCollaborativeSearch(int id)
        {
            var eventt = Events[id];
            if (eventt.Status == EventStatusEnum.InEmergency)
            {
                AreYouOkCommand.StartCollaborativeSearch(eventt);
            }


        }

        public IList<EventMonitorUserInfo> EventMonitorUsers(int eventId)
        {
            var eventt = Events[eventId];
            
            var sql = @"EXEC EventMonitorUsers :eventId; ";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("eventId", eventt.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventMonitorUserInfo)));

            return list.List<EventMonitorUserInfo>();

        }

        public class CreateOrEditParameters
        {
           

            public string Name { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Zip { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string FloorAndDepartament { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public DateTime BeginDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public int OrganizerId { get; set; }
            public List<int> UsersIds { get; set; }
        }
    }
}