using System.Collections.Generic;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports.Notifications
{
    public class NotificationAccessException : IIdentifiable
    {
        protected NotificationAccessException()
        {
            
        }
        public NotificationAccessException(Notification notification)
        {
            this.AssociatedNotification = notification;
        }

        public virtual Notification AssociatedNotification { get; set; }
        public virtual IList<BaseUser> Users { get; set; }
        public virtual int Id { get; protected set; }
    }
}