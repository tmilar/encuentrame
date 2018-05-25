using Encuentrame.Model.SoughtPersons;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.SoughtPersons
{
    public class SoughtPersonAnswerMap:MappingOf<SoughtPersonAnswer>
    {
        public SoughtPersonAnswerMap()
        {
            References(x => x.SourceUser);
            References(x => x.TargetUser);
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
            Map(x => x.Seen);
            Map(x => x.When).Nullable().Column("seenwhen");
            Map(x => x.IsOk ).Nullable();
        }
    }
}
