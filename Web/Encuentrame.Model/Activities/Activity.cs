using System;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Events;
using Encuentrame.Support;

namespace Encuentrame.Model.Activities
{
    public class Activity : IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual User User { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual DateTime BeginDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual Event Event { get; set; }
    }
}
