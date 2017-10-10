﻿using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
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

        public void Reply(int id,ReplyParameters parameters)
        {
            var areYouOk = AreYouOks[id];
            areYouOk.IAmOk = parameters.IAmOk;
            areYouOk.ReplyDatetime = SystemDateTime.Now;

            

        }
        public void Ask(AskParameters parameters)
        {
            var areYouOk = new AreYouOk();
            areYouOk.Sender = Users[parameters.SenderId];
            areYouOk.Target = Users[parameters.TargetId];
            areYouOk.IAmOk = false;

            AreYouOks.Put(areYouOk);
        }

       

        public class ReplyParameters
        {
            public bool IAmOk { get; set; }
        }

        public class AskParameters
        {
            public int SenderId { get; set; }
            public int TargetId { get; set; }
        }
    }
}