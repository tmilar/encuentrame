using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Activities;
using Encuentrame.Model.Events;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using Encuentrame.Support.ExpoNotification;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.AreYouOks
{
    [Lemming]
    public class AreYouOkCommand : BaseCommand, IAreYouOkCommand
    {
        [Inject]
        public IBag<AreYouOkActivity> AreYouOkActivities { get; set; }

   

        [Inject]
        public IBag<AreYouOkEvent> AreYouOkEvents { get; set; }
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

            var areYouOkActivities = AreYouOkActivities.Where(x=>x.ReplyDatetime==null && x.Target==replyUser );
            
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