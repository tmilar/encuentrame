using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    public interface IAuditContextManager
    {
        AuditContext Context { get; }
        IAuditContextManager Add(string key, string data);
        IAuditContextManager SetEntityId(int id);
        IAuditContextManager SetEntityType(Type type);
        IAuditContextManager SetAction(ActionsEnum action);
        IAuditContextManager AddAudit();
        IAuditContextManager Cancel();
        IAuditContextManager SetObject(IDisplayable displayable);
    }
}