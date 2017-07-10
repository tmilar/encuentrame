using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.Supports.Audits
{
    [Lemming]
    public class AuditCommand : IAuditCommand
    {
        [Inject]
        public IBag<Audit> Audits { get; set; }

        public IList<Audit> List()
        {
            return Audits.ToList();
        }
    }
}
