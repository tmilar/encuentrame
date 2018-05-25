using System;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Model.Supports.Audits
{
    public class AuditAttribute : Attribute, IAuditAttribute
    {
        public ActionsEnum BehaviorType { get; set; }
        public string IdField { get; set; }
        public Type EntityType { get; set; }
    }
}