using System;
using Encuentrame.Support;

namespace Encuentrame.Model.Positions
{
    public class Position:IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime Creation { get; set; }
    }
}
