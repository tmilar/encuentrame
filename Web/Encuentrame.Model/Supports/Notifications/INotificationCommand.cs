using System.Collections.Generic;

namespace Encuentrame.Model.Supports.Notifications
{
    public interface INotificationCommand
    {
        IList<Notification> GetList();
        IList<NotificationAccess> GetNotificationAccesses();
        void UpdateNotificationAccesses(NotificationCommand.CreateOrEditParameters notificationAccessParameters);
        IList<NotificationAccess> GetNotificationAccessesForCurrentUser();
        IList<NotificationAccessException> GetNotificationAccessExceptionsForCurrentUser();
        void UpdateExceptionsForUser(IEnumerable<int> exceptions);
    }
}
