using System.Collections.Generic;

namespace Encuentrame.Model.Supports.Audits
{
    public interface IAuditCommand
    {
        IList<Audit> List();
    }
}