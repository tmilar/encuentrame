using System;
using Encuentrame.Web.Models.Apis.Commons;

namespace Encuentrame.Web.Models.Apis.Events
{
    public class EventsApiResultModel
    {
        public  int Id { get;  set; }
        public  string Name { get; set; }
        public AddressModel Address { get; set; }
        public  decimal Latitude { get; set; }
        public  decimal Longitude { get; set; }
        public  DateTime BeginDateTime { get; set; }
        public  DateTime EndDateTime { get; set; }
    }
}