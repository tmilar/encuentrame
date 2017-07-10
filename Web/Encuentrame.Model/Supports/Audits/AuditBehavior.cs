using System;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    /// <summary>
    ///   Behavior that logs method invocation
    /// </summary>
    public class AuditBehavior : ILemmingBehavior
    {
        [Inject]
        public static IBag<Audit> Audits { get; set; }

        [Inject]
        public static IAuditContextManager AuditContextManager { get; set; }

        [Inject]
        public static IAuthenticationProvider AuthenticationProvider { get; set; }

        [Inject]
        public static INotificationService NotificationService { get; set; }
        #region ILemmingBehavior Members

        /// <summary>
        ///   Invocation of the interceptor
        /// </summary>
        /// <param name = "invocation">The invocation info.</param>
        /// <returns>return value of the invoked method</returns>
        public object ApplyTo(MethodInvocationInfo invocation)
        {
            object result;

            var attributes = invocation.Method.GetCustomAttributes(typeof(IAuditAttribute), true);
            Audit audit = null;           
            ActionsEnum? actionType = null;
            Type type = null;

            if (attributes.Length > 0)
            {
                audit = GetNewAudit();
                var auditAttribute = (IAuditAttribute)attributes[0];
                if (auditAttribute.BehaviorType != default(ActionsEnum))
                {
                    audit.Action = (int)auditAttribute.BehaviorType;
                    AuditContextManager.SetAction(auditAttribute.BehaviorType);
                    actionType = auditAttribute.BehaviorType;
                }
                if (auditAttribute.EntityType != null)
                {
                    audit.EntityType = auditAttribute.EntityType.Name;
                    AuditContextManager.SetEntityType(auditAttribute.EntityType);
                    type = auditAttribute.EntityType;
                }

                var entityIdParameterName = auditAttribute.IdField;
                if (!string.IsNullOrEmpty(entityIdParameterName))
                {
                    var parameters = invocation.Method.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameterInfo = parameters[i];
                        if (parameterInfo.Name == entityIdParameterName)
                        {
                            audit.EntityId = (int)invocation.Arguments[i];
                        }
                    }
                }
            }

            result = invocation.Proceed();

            var auditContext = AuditContextManager.Context;
            if (!auditContext.Cancelled)
            {
                if (AuditContextManager.Context != null && auditContext.ContextData.Count > 0)
                {
                    if (audit == null)
                        audit = GetNewAudit();
                    var contextData = auditContext.ContextData[0];
                    UpdateAudit(audit, contextData);
                    if (contextData.Action != default(ActionsEnum))
                        actionType = contextData.Action;
                    if (contextData.EntityType != null)
                        type = contextData.EntityType;
                }

                Audits.Put(audit);

                if (AuditContextManager.Context != null && auditContext.ContextData.Count > 1)
                {
                    for (int i = 1; i < auditContext.ContextData.Count; i++)
                    {
                        var contextData = auditContext.ContextData[i];
                        var extraAudit = new Audit(AuthenticationProvider.GetLoggedUser());
                        extraAudit.Date = audit.Date;
                        UpdateAudit(extraAudit, contextData);
                        Audits.Put(extraAudit);
                    }
                }
            }

            if (actionType.HasValue && type != null 
                && AuditContextManager.Context != null && auditContext.ContextData.Count > 0
                && auditContext.ContextData[0].Displayable!=null)
            {
                if (actionType == ActionsEnum.Create)
                    NotificationService.SendCreatedNotificationFor(type, auditContext.ContextData[0].Displayable.ToDisplay());

                if (actionType == ActionsEnum.Edit)
                    NotificationService.SendUpdatedNotificationFor(type, auditContext.ContextData[0].Displayable.ToDisplay(), audit.EntityId);

                if (actionType == ActionsEnum.Delete)
                    NotificationService.SendDeletedNotificationFor(type, auditContext.ContextData[0].Displayable.ToDisplay(), audit.EntityId);
            }

            return result;
        }

        private Audit GetNewAudit()
        {
            var audit = new Audit(AuthenticationProvider.GetLoggedUser());
            audit.Date = SystemDateTime.Now;
            return audit;
        }

        private void UpdateAudit(Audit audit, AuditContextData contextData)
        {
            if (contextData.EntityType != null)
                audit.EntityType = contextData.EntityType.Name;
            if (contextData.EntityId != default(int))
                audit.EntityId = contextData.EntityId;
            if (contextData.Action != default(ActionsEnum))
                audit.Action = (int)contextData.Action;
            if (contextData.ExtraData != null)
            {
                foreach (var keyValuePair in contextData.ExtraData)
                {
                    audit.AuditExtraDatas.Add(new AuditExtraData() { DataKey = keyValuePair.Key, DataValue = keyValuePair.Value });
                }
            }
        }

        #endregion
    }
}