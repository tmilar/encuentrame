using System;
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
    }
}

