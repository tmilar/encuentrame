using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    public class AuditContext
    {
        public bool Cancelled { get; set; }
        public IList<AuditContextData> ContextData { get; set; }

        public AuditContext()
        {
            ContextData = new List<AuditContextData>();
        }

        public void Cancel()
        {
            this.Cancelled = true;
        } 
    }

    public class AuditContextData
    {
        public int EntityId { get; set; }
        public Type EntityType { get; set; }
        public ActionsEnum Action { get; set; }
        public IList<KeyValuePair<string, string>> ExtraData { get; set; }
        public IDisplayable Displayable { get; set; }

        public AuditContextData()
        {
            ExtraData = new List<KeyValuePair<string, string>>();
        }               
    }
}