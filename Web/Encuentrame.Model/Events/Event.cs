using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Addresses;
using Encuentrame.Support;

namespace Encuentrame.Model.Events
{
    public class Event : IDeleteable
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }

        private Address _address;

        public virtual Address Address
        {
            get => _address;
            set => _address = value ?? new Address();
        }


        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual DateTime? DeletedKey { get; set; }
        public virtual DateTime BeginDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual EventStatusEnum Status  { get; set; }
        public virtual User Organizer { get; set; }

        private IList<User> _users;
        public virtual IList<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }
    }
}

