using System;

namespace Encuentrame.Model.Events
{
    public class EventMonitorPositionInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int IAmOk { get; set; }
        public DateTime ReplyDatetime { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public DateTime LastPositionUpdate { get; set; }
    }
}