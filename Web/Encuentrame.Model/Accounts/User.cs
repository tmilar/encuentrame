using System;
using Encuentrame.Support;

namespace Encuentrame.Model.Accounts
{
    public abstract class BaseUser : IDeleteable, IDisplayable
    {
        public virtual int Id { get; protected set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }

        public virtual string FullName
        {
            get { return "{0}, {1}".FormatWith(LastName, FirstName); }
        }

        public virtual string Email { get; set; }
        public virtual string EmailAlternative { get; set; }

        public virtual string InternalNumber { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual string Image { get; set; }
        public virtual Role Role { get; set; }
        public virtual DateTime? DeletedKey { get; set; }
        public virtual string ToDisplay()
        {
            return FullName;
        }
    }

    public class User:BaseUser
    {
        
    }

    public class SystemUser : BaseUser
    {

    }
}