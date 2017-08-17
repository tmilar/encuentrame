using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support.Mappings;
using System.Linq;

namespace Encuentrame.Model.Mappings.Support
{
    public class NotificationAccessMap : MappingOf<NotificationAccess>
    {
        public NotificationAccessMap()
        {
            Map(x => x.AllowEveryone);
            References(x => x.AssociatedNotification).Unique().Cascade.All();
            HasManyToMany(x => x.Users);
            
        }
    }
}
