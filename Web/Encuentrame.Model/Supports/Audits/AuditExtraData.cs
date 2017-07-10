using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Audits
{
    public class AuditExtraData : IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual string DataKey { get; set; }
        public virtual string DataValue { get; set; }
    }
}