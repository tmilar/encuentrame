using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Models.Notifications
{
    public class MyNotificationAccessRowInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Allow { get; set; }
    }

    public class MyNotificationsModel
    {
        public IList<MyNotificationAccessRowInfo> NotificationAccessRowInfos { get; set; }

        public MyNotificationsModel()
        {
            NotificationAccessRowInfos = new List<MyNotificationAccessRowInfo>();
        }
    }
}