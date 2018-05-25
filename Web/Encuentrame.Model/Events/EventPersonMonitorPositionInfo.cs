using System;

namespace Encuentrame.Model.Events
{
    public class EventPersonMonitorPositionInfo
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
       
    }
}