using System;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.Session;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    [Lemming]
    public class AuditContextManager : IAuditContextManager
    {
        private const string AuditContextDataKey = "AuditContextData";

        [Inject]
        public IExecutionContext ExecutionContext { get; set; }

        [Inject]
        public AuditContext Context
        {
            get
            {
                var context = ExecutionContext.GetObject<AuditContext>(AuditContextDataKey);
                if (context == null)
                {
                    context = new AuditContext();
                    ExecutionContext.SetObject(AuditContextDataKey, context);
                }
                return context;
            }
        }

        public AuditContextData CurrentContextData
        {
            get
            {
                if (Context.ContextData.Count == 0)
                {
                    var currentContextData = new AuditContextData();
                    Context.ContextData.Add(currentContextData);
                }
                return Context.ContextData.Last();
            }
        }

        public IAuditContextManager Add(string key, string data)
        {
            CurrentContextData.ExtraData.Add(new KeyValuePair<string, string>(key, data));
            return this;
        }

        public IAuditContextManager SetEntityId(int id)
        {
            CurrentContextData.EntityId = id;
            return this;
        }

        public IAuditContextManager SetEntityType(Type type)
        {
            CurrentContextData.EntityType = type;
            return this;
        }

        public IAuditContextManager SetObject(IDisplayable displayable)
        {
            CurrentContextData.Displayable = displayable;
            return this;
        }

        public IAuditContextManager SetAction(ActionsEnum action)
        {
            CurrentContextData.Action = action;
            return this;
        }

        public IAuditContextManager AddAudit()
        {
            var newAudit = new AuditContextData
            {
                EntityType = CurrentContextData.EntityType,
                Action = CurrentContextData.Action
            };
            Context.ContextData.Add(newAudit);
            return this;
        }

        public IAuditContextManager Cancel()
        {
            Context.Cancel();
            return this;
        }
    }
}