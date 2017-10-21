
using Encuentrame.Model.Events;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Events
{
    public class EventMap:MappingOf<Event>
    {
        public EventMap()
        {
            Map(x => x.Name).Not.Nullable().UniqueKey("nameUnique");
            Map(x => x.DeletedKey).UniqueKey("nameUnique");
            Map(x => x.Latitude);
            Map(x => x.Longitude);
            Map(x => x.BeginDateTime);
            Map(x => x.EndDateTime);
            Map(x => x.Status).Not.Nullable();
            Component(x => x.Address, m =>
            {
                m.Map(x => x.Number);
                m.Map(x => x.Street);
                m.Map(x => x.City);
                m.Map(x => x.Province );
                m.Map(x => x.FloorAndDepartament);
                m.Map(x => x.Zip);
            }).ColumnPrefix("address");
            References(x => x.Organizer).Not.Nullable();
            HasManyToMany(x => x.Users).Table("EventsUsers");
        }
    }
}
