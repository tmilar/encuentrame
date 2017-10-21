using System;
using System.Collections.Generic;
using Encuentrame.Model.Contacts;
using Encuentrame.Model.Devices;
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
        public virtual RoleEnum Role { get; set; }

      
       
        public virtual DateTime? DeletedKey { get; set; }
        public virtual string ToDisplay()
        {
            return FullName;
        }
    }

    public class User:BaseUser
    {
        private IList<Device> _devices;
        public virtual IList<Device> Devices
        {
            get { return _devices ?? (_devices = new List<Device>()); }
            set { _devices = value; }
        }

        private IList<Contact> _contacts;
        public virtual IList<Contact> Contacts
        {
            get { return _contacts ?? (_contacts = new List<Contact>()); }
            set { _contacts = value; }
        }

    }

    public class SystemUser : BaseUser
    {

    }
}