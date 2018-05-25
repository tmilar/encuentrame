using Encuentrame.Model.AreYouOks;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.AreYouOks
{
    public class BaseAreYouOkMap : MappingOf<BaseAreYouOk>
    {
        public BaseAreYouOkMap()
        {
            Map(x => x.Created).Not.Nullable();
           
            References(x => x.Target).Not.Nullable();
            Map(x => x.IAmOk);
            Map(x => x.ReplyDatetime).Nullable();
            DiscriminateSubClassesOnColumn("AreYouOkType");
        }
    }
    public class AreYouOkEventMap : SubMappingOf<AreYouOkEvent>
    {
        public AreYouOkEventMap()
        {
            DiscriminatorValue(typeof(AreYouOkEvent).Name);
            References(x => x.Event);
        }
    }
    public class AreYouOkActivityMap : SubMappingOf<AreYouOkActivity>
    {
        public AreYouOkActivityMap()
        {
            DiscriminatorValue(typeof(AreYouOkActivity).Name);
            References(x => x.Sender);
        }
    }

}
