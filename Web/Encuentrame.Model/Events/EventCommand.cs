﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Model.Positions;
using Encuentrame.Model.SoughtPersons;
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
        public IBag<Position> Positions { get; set; }


        [Inject]
        public IBag<SoughtPersonAnswer> SoughtPersonAnswers { get; set; }



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
            eventt.Address = new Address()
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
            var eventt = Events[id];
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
            if (eventt.Status.In(EventStatusEnum.InEmergency))
            {
                eventt.Status = EventStatusEnum.InProgress;
                eventt.EmergencyDateTime = null;
                AreYouOkCommand.CancelAskFromEvent(eventt);
            }
            else
            {
                throw new DeclareEmergencyException();
            }
        }

        public void BeginEvent(int id)
        {
            var eventt = Events[id];
            eventt.Status = EventStatusEnum.InProgress;
            eventt.BeginDateTime = SystemDateTime.Now;
            //TODO: hacer las notifications
        }

        public void CancelFinalizeEvent(int id)
        {
            var eventt = Events[id];
            if (eventt.EmergencyDateTime != null)
            {
                eventt.Status = EventStatusEnum.InEmergency;
            }
            else
            {
                eventt.Status = EventStatusEnum.InProgress;
            }
            
            eventt.EndDateTime = eventt.EndDateTime.AddHours(4);
            //TODO: hacer las notifications
        }

        public void FinalizeEvent(int id)
        {
            var eventt = Events[id];
            eventt.Status = EventStatusEnum.Completed;
            eventt.EndDateTime = SystemDateTime.Now.AddHours(1);
            //TODO: hacer las notifications
        }

        public void StartCollaborativeSearch(int id)
        {
            var eventt = Events[id];
            if (eventt.Status == EventStatusEnum.InEmergency)
            {
                eventt.CollaborativeSearchDateTime = SystemDateTime.Now;
                AreYouOkCommand.StartCollaborativeSearch(eventt);
            }


        }

        public IList<EventMonitorUserInfo> EventMonitorUsers(int eventId)
        {
            var eventt = Events[eventId];

            var sql = @"EXEC EventMonitorUsers :eventId, :from; ";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("from", eventt.BeginDateTime)
                .SetParameter("eventId", eventt.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventMonitorUserInfo)));

            return list.List<EventMonitorUserInfo>();

        }

        public IList<EventMonitorPositionInfo> PositionsFromEvent(int eventId, DateTime? datetimeTo)
        {
            var eventt = Events[eventId];

            var sql = @"EXEC EventMonitorPositions :eventId, :datetimeTo; ";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("eventId", eventt.Id)
                .SetParameter("datetimeTo", datetimeTo ?? SystemDateTime.Now.AddDays(1))
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventMonitorPositionInfo)));

            return list.List<EventMonitorPositionInfo>();
        }

        public IList<EventPersonMonitorPositionInfo> PositionsUserFromEvent(int eventId, int userId)
        {
            var eventt = Events[eventId];
            var user = Users[userId];

            DateTime to = SystemDateTime.Now;
            if (eventt.Status == EventStatusEnum.InProgress || eventt.Status == EventStatusEnum.InEmergency)
            {
                to = SystemDateTime.Now;
            }
            else if (eventt.Status == EventStatusEnum.Completed)
            {
                to = eventt.EndDateTime;
            }
            else
            {
                return new List<EventPersonMonitorPositionInfo>();
            }

            return Positions.Where(x => x.UserId == user.Id && x.Creation >= eventt.BeginDateTime && x.Creation <= to)
                .OrderBy(x => x.Creation)
                .Select(x => new EventPersonMonitorPositionInfo()
                {
                    Id = x.Id,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
                    Datetime = x.Creation
                }).ToList();


        }

        public EventPersonStatusInfo GetEventPersonStatus(int eventId)
        {
            var eventt = Events[eventId];

            var sql = @"select	SUM(iif(baa.ReplyDatetime is null,0,  iif(baa.IAmOk=0 , 0 , 1) ) ) AS Ok,
		                        SUM(iif(baa.ReplyDatetime is null,0,  iif(baa.IAmOk=0 , 1 , 0) ) ) AS NotOk,
		                        SUM(iif(baa.ReplyDatetime is null,1, 0 ) ) AS WithoutAnswer
                        from activities act
	                        left join BaseAreYouOks baa on act.User_id=baa.Target_id
	                        where act.Event_id=:eventId
	                        group by act.Event_id";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("eventId", eventt.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventPersonStatusInfo)));

            return list.UniqueResult<EventPersonStatusInfo>();
        }

        public EventSeenNotSeenInfo GetEventSeenNotSeen(int eventId)
        {
            var eventt = Events[eventId];

            var sql = @"select	SUM(iif(spa.seen is null,0,  iif(spa.seen=0 , 0 , 1) ) ) AS Seen,
		                    SUM(iif(spa.seen is null,0,  iif(spa.seen=0 , 1 , 0) ) ) AS NotSeen,
		                    SUM(iif(spa.seen is null,1, 0 ) ) AS WithoutAnswer
                      from activities act
                      inner join events ev on ev.Id=act.Event_id
	                    left join SoughtPersonAnswers spa on act.User_id=spa.SourceUser_id AND  spa.seenwhen>=ev.BeginDateTime AND spa.seenwhen<=ev.EndDateTime
	                    where act.Event_id=:eventId 
	                    group by act.Event_id";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("eventId", eventt.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventSeenNotSeenInfo)));

            return list.UniqueResult<EventSeenNotSeenInfo>();
        }

        public EventSeenOkNotOkInfo GetEventOkNotOk(int eventId)
        {
            var eventt = Events[eventId];

            var sql = @"select	SUM(iif(spa.IsOk is null,0,  iif(spa.IsOk=0 , 0 , 1) ) ) AS SeenOk,
		                        SUM(iif(spa.IsOk is null,0,  iif(spa.IsOk=0 , 1 , 0) ) ) AS SeenNotOk,
		                        SUM(iif(spa.IsOk is null,1, 0 ) ) AS WithoutAnswer
                          from activities act
                          inner join events ev on ev.Id=act.Event_id
	                        left join SoughtPersonAnswers spa on act.User_id=spa.SourceUser_id AND spa.seen=1 AND  spa.seenwhen>=ev.BeginDateTime AND spa.seenwhen<=ev.EndDateTime
	                        where act.Event_id=:eventId 
	                        group by act.Event_id;";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("eventId", eventt.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(EventSeenOkNotOkInfo)));

            return list.UniqueResult<EventSeenOkNotOkInfo>();
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