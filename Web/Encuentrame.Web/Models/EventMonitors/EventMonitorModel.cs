using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Model.Events;
using Encuentrame.Web.Helpers;

namespace Encuentrame.Web.Models.EventMonitors
{
    public class EventMonitorModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Name")]
        public string Name { get; set; }

        [Editable(false)]
        [Display(ResourceType = typeof(Translations), Name = "Status")]
        public EventStatusEnum Status { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Latitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal Latitude { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Longitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal Longitude { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "BeginDateTime")]
        public DateTime BeginDateTime { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EndDateTime")]
        public DateTime EndDateTime { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Address")]
        public  string Address { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Organizer")]
        [Editable(false)]
        public string OrganizerDisplay { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EmergencyDateTime")]
        public DateTime EmergencyDateTime { get; set; }



        [Display(ResourceType = typeof(Translations), Name = "IAmOk")]
        [UIHint("bool")]
        public bool IAmOk { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "IAmNotOk")]
        [UIHint("bool")]
        public bool IAmNotOk { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "WithoutAnswer")]
        [UIHint("bool")]
        public bool WithoutAnswer { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "LastUpdate")]
        [UIHint("datetime")]
        public DateTime LastUpdate { get; set; }


    }
}