using Encuentrame.Model.Activities;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Activities
{
    public class ActivityMap : MappingOf<Activity>
    {
        public ActivityMap()
        {
            References(x => x.User).Not.Nullable(); 
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Latitude).Not.Nullable();
            Map(x => x.Longitude).Not.Nullable();
            Map(x => x.BeginDateTime).Not.Nullable();
            Map(x => x.EndDateTime).Not.Nullable();
            References(x => x.Event).Nullable();
        }
    }
}
