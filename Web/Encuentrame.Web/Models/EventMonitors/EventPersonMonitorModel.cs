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


        [Editable(false)]
        [Display(ResourceType = typeof(Translations), Name = "Status")]
        public IAmOkEnum Status{ get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Seen")]
        public int Seen { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "SeenOk")]
        public int SeenOk { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "SeenNotOk")]
        public int SeenNotOk { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "SeenWithoutAnswer")]
        public int SeenWithoutAnswer { get; set; }
    }
}