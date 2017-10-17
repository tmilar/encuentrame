using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
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
        public IBag<AreYouOk> AreYouOks { get; set; }
        
        [Inject]
        public IBag<User> Users { get; set; }
        
        public AreYouOk Get(int id)
        {
            return AreYouOks[id];
        }
        
        public IList<AreYouOk> List()
        {
            return AreYouOks.ToList();
        }
       
        public void Delete(int id)
        {
            var areYouOk = AreYouOks[id];
            AreYouOks.Remove(areYouOk);
        }

        public void Reply(ReplyParameters parameters)
        {
            var replyUser = Users[parameters.UserId];
            var areYouOks = AreYouOks.Where(x=>x.ReplyDatetime==null && x.Target==replyUser );
            
            foreach (var areYouOk in areYouOks)
            {
                areYouOk.IAmOk = parameters.IAmOk;
                areYouOk.ReplyDatetime = SystemDateTime.Now;

                var list = areYouOk.Sender.Devices.Select(x => new BodySend()
                {
                    Token = x.Token,
                    Body = parameters.IAmOk ? $"{replyUser.FullName} ha indicado que está bien!" : $"{replyUser.FullName} esta con algun problema",
                    Title = "Encuentrame",
                    Data = new
                    {
                        Id = areYouOk.Id,
                        TargetUserId = areYouOk.Target.Id,
                        Ok = parameters.IAmOk,
                        ReplyDatetime = areYouOk.ReplyDatetime,
                        Type = "Areyouok.Reply",
                    }
                }).ToList();

                ExpoPushHelper.SendPushNotification(list);

            }


        }
        public void Ask(AskParameters parameters)
        {
            var areYouOk = new AreYouOk();
            areYouOk.Sender = Users[parameters.SenderId];
            areYouOk.Target = Users[parameters.TargetId];
            areYouOk.IAmOk = false;

            AreYouOks.Put(areYouOk);

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