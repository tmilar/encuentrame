using Encuentrame.Model.AreYouOks;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.AreYouOks
{
    public class AreYouOkMap : MappingOf<AreYouOk>
    {
        public AreYouOkMap()
        {
            Map(x => x.Created).Not.Nullable();
            References(x => x.Sender).Not.Nullable();
            References(x => x.Target).Not.Nullable();
            Map(x => x.IAmOk);
            Map(x => x.ReplyDatetime).Nullable();
        }
    }
}
