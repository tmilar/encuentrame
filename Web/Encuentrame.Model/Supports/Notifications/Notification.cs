using Encuentrame.Model.Accounts;
using Encuentrame.Support;
using Encuentrame.Support.Email;

namespace Encuentrame.Model.Supports.Notifications
{
    public abstract class Notification : IIdentifiable, IDisplayable
    {
        public virtual int Id { get; protected set; }
        public abstract string TemplateName { get; }
        public virtual string NotificationType { get; set; }
        public virtual NotificationAccess NotificationAccess { get; set; }
        public virtual NotificationAccessException NotificationAccessException { get; set; }

        public virtual string ToDisplay()
        {
            return this.GetType().Name;
        }
    }

    public abstract class TypedNotification<T> : Notification where T: class, IIdentifiable
    {
        
    }

   

    public class RoleCreatedNotification : TypedNotification<Role>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectCreatedTemplateName; }
        }
    }
    public class UserCreatedNotification : TypedNotification<User>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectCreatedTemplateName; }
        }
    }

    public class UserUpdatedNotification : TypedNotification<User>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectUpdatedTemplateName; }
        }
    }

  
    public class RoleUpdatedNotification : TypedNotification<Role>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectUpdatedTemplateName; }
        }
    }


    public class UserDeletedNotification : TypedNotification<User>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectDeletedTemplateName; }
        }
    }


    public class RoleDeletedNotification : TypedNotification<Role>
    {
        public override string TemplateName
        {
            get { return EmailTemplateManager.ObjectDeletedTemplateName; }
        }
    }
    
}
