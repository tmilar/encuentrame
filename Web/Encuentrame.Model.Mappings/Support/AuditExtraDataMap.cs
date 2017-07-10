using Encuentrame.Model.Supports;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Support
{
    public class AuditExtraDataMap : MappingOf<AuditExtraData>
    {
        public AuditExtraDataMap()
        {
            Map(x => x.DataKey);
            Map(x => x.DataValue);
        }
    }
}