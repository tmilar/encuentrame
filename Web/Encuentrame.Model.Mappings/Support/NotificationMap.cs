using Encuentrame.Model.Supports.Notifications;
using Encuentrame.Support;
using Encuentrame.Support.Mappings;
using System.Linq;

namespace RAEI.ProgramacionDeProduccion.Model.Mappings.Support
{
    public class NotificationMap : MappingOf<Notification>
    {
        public NotificationMap()
        {
            DiscriminateSubClassesOnColumn("NotificationType");
            HasOne(x => x.NotificationAccess).Cascade.All().PropertyRef(x => x.AssociatedNotification);
            HasOne(x => x.NotificationAccessException).Cascade.All().PropertyRef(x => x.AssociatedNotification);
        }
    }

    public abstract class NotificationMappingOf<T> : SubMappingOf<T> where T : Notification, IIdentifiable
    {
        protected NotificationMappingOf()
        {
            DiscriminatorValue(GetDiscriminatorValue());
        }

        public string GetDiscriminatorValue()
        {
            var notificationType = GetType().BaseType.GetGenericArguments().Single();
            return notificationType.Name;
        }
    }

  

    public class UserCreatedNotificationMap : NotificationMappingOf<UserCreatedNotification>
    {
    }

  

  

  
    public class UserUpdatedNotificationMap : NotificationMappingOf<UserUpdatedNotification>
    {
    }

   

 

  

    public class UserDeletedNotificationMap : NotificationMappingOf<UserDeletedNotification>
    {
    }

   

 
}
