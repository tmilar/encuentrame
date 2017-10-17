using System;

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

     
        public virtual string City { get; set; }
        public virtual ItemModel Organizer { get; set; }

    }
}