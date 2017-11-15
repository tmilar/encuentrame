using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Activities;
using Encuentrame.Model.Events;
using Encuentrame.Model.Positions;
using Encuentrame.Model.SoughtPersons;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using Encuentrame.Support.ExpoNotification;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Transform;

namespace Encuentrame.Model.AreYouOks
{
    [Lemming]
    public class AreYouOkCommand : BaseCommand, IAreYouOkCommand
    {
        [Inject]
        public IBag<AreYouOkActivity> AreYouOkActivities { get; set; }

        [Inject]
        public IBag<Event> Events { get; set; }

        [Inject]
        public IBag<SoughtPersonAnswer> SoughtPersonAnswers { get; set; }


        [Inject]
        public IBag<AreYouOkEvent> AreYouOkEvents { get; set; }

        [Inject]
        public IBag<Position> Positions { get; set; }

        [Inject]
        public IBag<Activity> Activities { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        public AreYouOkActivity Get(int id)
        {
            return AreYouOkActivities[id];
        }

        public IList<AreYouOkActivity> List()
        {
            return AreYouOkActivities.ToList();
        }

        public void Delete(int id)
        {
            var areYouOk = AreYouOkActivities[id];
            AreYouOkActivities.Remove(areYouOk);
        }

        public void Reply(ReplyParameters parameters)
        {
            var replyUser = Users[parameters.UserId];

            var areYouOkActivities = AreYouOkActivities.Where(x => x.ReplyDatetime == null && x.Target == replyUser);

            foreach (var areYouOkActivity in areYouOkActivities)
            {
                areYouOkActivity.IAmOk = parameters.IAmOk;
                areYouOkActivity.ReplyDatetime = SystemDateTime.Now;

                var list = areYouOkActivity.Sender.Devices.Select(x => new BodySend()
                {
                    Token = x.Token,
                    Body = parameters.IAmOk ? $"{replyUser.FullName} ha indicado que está bien!" : $"{replyUser.FullName} esta con algun problema",
                    Title = "Encuentrame",
                    Data = new
                    {
                        Id = areYouOkActivity.Id,
                        TargetUserId = areYouOkActivity.Target.Id,
                        Ok = parameters.IAmOk,
                        ReplyDatetime = areYouOkActivity.ReplyDatetime,
                        Type = "Areyouok.Reply",
                    }
                }).ToList();

                ExpoPushHelper.SendPushNotification(list);

            }

            var areYouOkEvents = AreYouOkEvents.Where(x => x.ReplyDatetime == null && x.Target == replyUser);

            foreach (var areYouOkEvent in areYouOkEvents)
            {
                areYouOkEvent.IAmOk = parameters.IAmOk;
                areYouOkEvent.ReplyDatetime = SystemDateTime.Now;

            }



        }
        public void Ask(AskParameters parameters)
        {
            var areYouOk = new AreYouOkActivity();
            areYouOk.Sender = Users[parameters.SenderId];
            areYouOk.Target = Users[parameters.TargetId];
            areYouOk.IAmOk = false;

            AreYouOkActivities.Put(areYouOk);

            CurrentUnitOfWork.Checkpoint();

            var list = areYouOk.Target.Devices.Select(x => new BodySend()
            {
                Token = x.Token,
                Body = "¿estas bien?",
                Title = "Encuentrame",
                Data = new
                {
                    Id = areYouOk.Id,
                    SenderUserId = areYouOk.Target.Id,
                    AskDatetime = areYouOk.Created,
                    Type = "Areyouok.Ask",
                }
            }).ToList();

            ExpoPushHelper.SendPushNotification(list);

        }


        public void AskFromEvent(Event eventt)
        {

            var activities = Activities.Where(x => x.Event == eventt);

            foreach (var activity in activities)
            {
                var areYouOk = new AreYouOkEvent();
                areYouOk.Event = eventt;
                areYouOk.Target = activity.User;
                areYouOk.IAmOk = false;

                AreYouOkEvents.Put(areYouOk);
            }


            CurrentUnitOfWork.Checkpoint();

            var listAsks = AreYouOkEvents.Where(x => x.Event == eventt);
            foreach (var areYouOkEvent in listAsks)
            {
                var list = areYouOkEvent.Target.Devices.Select(x => new BodySend()
                {
                    Token = x.Token,
                    Body = "¿estas bien?",
                    Title = "Encuentrame",
                    Data = new
                    {
                        Id = areYouOkEvent.Id,
                        SenderUserId = areYouOkEvent.Target.Id,
                        AskDatetime = areYouOkEvent.Created,
                        Type = "Areyouok.Ask",
                    }
                }).ToList();

                ExpoPushHelper.SendPushNotification(list);
            }



        }
        public void StartCollaborativeSearch(Event eventt)
        {
            var searchers = AreYouOkEvents.Where(x => x.Event == eventt && x.ReplyDatetime != null).ToList();

            foreach (var areYouOkEvent in searchers)
            {
                var list = areYouOkEvent.Target.Devices.Select(x => new BodySend()
                {
                    Token = x.Token,
                    Body = $"Se declaro una emergencia en el evento {eventt.Name}",
                    Title = "Encuentrame",
                    Data = new
                    {
                        EventId = eventt.Id,
                        EmergencyDateTime = eventt.EmergencyDateTime.Value,
                        Type = "Event.StartCollaborativeSearch",
                    }
                }).ToList();

                ExpoPushHelper.SendPushNotification(list);
            }

        }

        public IEnumerable<SoughtPersonInfo> SoughtPeople(User userSearcher)
        {

            var currentActivity = Activities
                .Where(x => x.Event != null && x.User == userSearcher && x.Event.Status == EventStatusEnum.InEmergency)
                .FirstOrDefault();

            if (currentActivity == null)
            {
                return new List<SoughtPersonInfo>();
            }

            var sql = @"EXEC SoughtPeople :userId, :eventId; ";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("userId", userSearcher.Id)
                .SetParameter("eventId", currentActivity.Event.Id)
                .SetResultTransformer(Transformers.AliasToBean(typeof(SoughtPersonInfo)));

            return list.List<SoughtPersonInfo>();

        }

        public void SoughtPersonSeen(SoughtPersonSeenParameters parameters)
        {
            decimal? latitude = null;
            decimal? longitude = null;
            var sourceUser = Users[parameters.SourceUserId];
            var targetUser = Users[parameters.TargetUserId];

            if (parameters.When.HasValue)
            {
                Positions.Where(c => c.UserId == parameters.TargetUserId && c.Creation <= parameters.When)
                    .OrderByDescending(x => x.Creation).Take(1).ForEach(i =>
                    {
                        latitude = i.Latitude;
                        longitude = i.Longitude;
                    });
            }
            else
            {
                parameters.When = SystemDateTime.Now;
                Positions.Where(c => c.UserId == parameters.SourceUserId && c.Creation <= parameters.When)
                    .OrderByDescending(x => x.Creation).Take(1).ForEach(i =>
                    {
                        latitude = i.Latitude;
                        longitude = i.Longitude;
                    });
            }

            var soughtPersonAswer = new SoughtPersonAnswer()
            {
                When = parameters.When,
                Seen = true,
                Latitude = latitude,
                Longitude = longitude,
                IsOk = parameters.IsOk,
                SourceUser = sourceUser,
                TargetUser = targetUser,
            };

            SoughtPersonAnswers.Put(soughtPersonAswer);

        }

        public void SoughtPersonDismiss(SoughtPersonDismissParameters parameters)
        {
            var soughtPersonAswer = new SoughtPersonAnswer()
            {
                When = parameters.When,
                Seen = false,
                SourceUser = Users[parameters.SourceUserId],
                TargetUser = Users[parameters.TargetUserId],
            };

            SoughtPersonAnswers.Put(soughtPersonAswer);
        }

        public IList<PositionWhereWasSeenInfo> PositionsWhereWasSeen(int eventId, int userId)
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
                return new List<PositionWhereWasSeenInfo>();
            }

            return SoughtPersonAnswers.Where(x => x.TargetUser == user && x.Seen == true && x.When >= eventt.BeginDateTime && x.When <= to)
                .OrderBy(x => x.When)
                .Select(x => new PositionWhereWasSeenInfo()
                {
                    Id = x.Id,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
                    When = x.When,
                    Status = x.IsOk.HasValue ? (x.IsOk.Value ? 20 : 10) : 0
                }).ToList();

        }

        public SeenInfo GetSeenInfo(int eventId, int userId)
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
                return new SeenInfo();
            }

            var sql = @"SELECT
                          COUNT(Seen) AS Seen,
                          SUM(CASE
                            WHEN IsOk IS NULL THEN 1
                            ELSE 0
                          END) AS SeenWithoutAnswer,
                          SUM(CASE
                            WHEN IsOk = 0 THEN 1
                            ELSE 0
                          END) AS SeenNotOk,
                          SUM(CASE
                            WHEN IsOk = 1 THEN 1
                            ELSE 0
                          END) AS SeenOk
                        FROM SoughtPersonAnswers
                        WHERE Seen = 1
                        AND TargetUser_id = :userId
                        AND seenwhen >= :from AND seenwhen<= :to
                        GROUP BY Seen";

            var list = NHibernateContext.CurrentSession.CreateSQLQuery(sql)
                .SetParameter("userId", user.Id)
                .SetParameter("from", eventt.BeginDateTime)
                .SetParameter("to", to)
                .SetResultTransformer(Transformers.AliasToBean(typeof(SeenInfo)));

            return list.UniqueResult<SeenInfo>();


        }


        public class SoughtPersonSeenParameters
        {
            public DateTime? When { get; set; }

            public int SourceUserId { get; set; }
            public int TargetUserId { get; set; }
            public bool? IsOk { get; set; }

        }

        public class SoughtPersonDismissParameters
        {
            public DateTime When { get; set; }
            public int SourceUserId { get; set; }
            public int TargetUserId { get; set; }
        }

        public class ReplyParameters
        {
            public bool IAmOk { get; set; }
            public int UserId { get; set; }
        }

        public class AskParameters
        {
            public int SenderId { get; set; }
            public int TargetId { get; set; }
        }



    }
}