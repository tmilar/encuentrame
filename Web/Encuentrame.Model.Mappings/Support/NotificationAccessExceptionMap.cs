using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support.Mappings;
using System.Linq;

namespace Encuentrame.Model.Mappings.Support
{
    public class NotificationAccessExceptionMap : MappingOf<NotificationAccessException>
    {
        public NotificationAccessExceptionMap()
        {
            References(x => x.AssociatedNotification).Unique().Cascade.All();
            HasManyToMany(x => x.Users);
        }
    }
}
