using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Web.Helpers;

namespace Encuentrame.Web.Models.EventMonitors
{
    public class EventPersonMonitorModel
    {
       
        public int EventId { get; set; }

        public int UserId { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Latitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal EventLatitude { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Longitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal EventLongitude { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EventName")]
        public string EventName { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "Username")]
        public string Username { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Image")]
        [UIHint("image")]
        public virtual string Image { get; set; }


    }
}