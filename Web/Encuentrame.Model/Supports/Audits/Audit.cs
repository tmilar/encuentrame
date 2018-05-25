using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    public class Audit : IIdentifiable
    {
        private IList<AuditExtraData> _auditExtraDatas;
        public virtual int Id { get; protected set; }
        public virtual DateTime Date { get; set; }
        public virtual int Action { get; set; }
        public virtual int EntityId { get; set; }
        public virtual string EntityType { get; set; }

        public virtual IList<AuditExtraData> AuditExtraDatas
        {
            get
            {
                if (_auditExtraDatas == null)
                    _auditExtraDatas = new List<AuditExtraData>();
                return _auditExtraDatas;
            }
            set { _auditExtraDatas = value; }
        }

        public virtual BaseUser User { get; set; }

        protected Audit()
        {

        }

        public Audit(BaseUser user)
        {
            User = user;
        }
    }
}