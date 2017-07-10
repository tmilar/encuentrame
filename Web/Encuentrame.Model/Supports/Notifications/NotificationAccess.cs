using System.Collections.Generic;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Notifications
{
    public class NotificationAccess: IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual bool AllowEveryone { get; set; }
        public virtual Notification AssociatedNotification { get; set; }
        public virtual IList<BaseUser> Users { get; set; }
        public virtual IList<Role> Roles { get; set; }

        public NotificationAccess()
        {
            
        }

        public NotificationAccess(Notification notification)
        {
            this.AssociatedNotification = notification;
        }
    }
}