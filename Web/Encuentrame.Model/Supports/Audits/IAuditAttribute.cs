using System;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Model.Supports.Audits
{
    public interface IAuditAttribute
    {
        ActionsEnum BehaviorType { get; set; }
        string IdField { get; set; }
        Type EntityType { get; set; }
    }
}