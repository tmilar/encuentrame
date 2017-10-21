using System;
using Encuentrame.Model.Events;

namespace Encuentrame.Web.Models.Events
{
    public class EventListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

       
        public decimal Latitude { get; set; }

       
        public decimal Longitude { get; set; }

        public DateTime BeginDateTime { get; set; }

       
        public DateTime EndDateTime { get; set; }

        public  EventStatusEnum Status { get; set; }

        public  string City { get; set; }
        public  ItemModel Organizer { get; set; }

    }
}