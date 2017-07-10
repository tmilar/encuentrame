using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Supports;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Support
{
    public class AuditMap : MappingOf<Audit>
    {
        public AuditMap()
        {
            Map(x => x.Date);
            Map(x => x.Action);
            Map(x => x.EntityId);
            Map(x => x.EntityType);
            References(x => x.User);
            HasMany(x => x.AuditExtraDatas).Cascade.AllDeleteOrphan();
        }
    }
}
