using Encuentrame.Model.Positions;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Positions
{
    public class PositionMap:MappingOf<Position>
    {
        public PositionMap()
        {
            Map(x => x.Latitude).Not.Nullable();
            Map(x => x.Longitude).Not.Nullable();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Creation).Not.Nullable();
            Map(x => x.Accuracy).Nullable();
            Map(x => x.Heading).Nullable();
            Map(x => x.Speed).Nullable();

        }
    }
}
