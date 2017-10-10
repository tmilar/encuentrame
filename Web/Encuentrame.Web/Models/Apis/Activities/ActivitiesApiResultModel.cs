using System;

namespace Encuentrame.Web.Models.Apis.Activities
{
    public class ActivitiesApiResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }
        
        public DateTime BeginDateTime { get; set; }
        
        public DateTime EndDateTime { get; set; }

        public int? EventId { get; set; }
    }
}