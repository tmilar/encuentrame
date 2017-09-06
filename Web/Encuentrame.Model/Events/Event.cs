using System;
using Encuentrame.Model.Addresses;
using Encuentrame.Support;

namespace Encuentrame.Model.Events
{
    public class Event : IDeleteable
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual Address Address { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual DateTime? DeletedKey { get; set; }
    }
}

