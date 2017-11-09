using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Models.EventMonitors
{
    public class EventMonitorUserListModel
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }

        public IAmOkEnum IAmOk { get; set; }

        public bool WasSeen { get; set; }
        public int Seen { get; set; }
        public int NotSeen { get; set; }

        public DateTime LastPositionUpdate { get; set; }
    }
}