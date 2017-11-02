using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Model.Events;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.MetadataProviders;

namespace Encuentrame.Web.Models.Events
{
    public class EventModel
    {

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "Name")]
        public string Name { get; set; }

        [Editable(false)]
        [Display(ResourceType = typeof(Translations), Name = "Status")]
        public EventStatusEnum Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-90, 90, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        [Display(ResourceType = typeof(Translations), Name = "Latitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal Latitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Range(-180, 180, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeError")]
        [Display(ResourceType = typeof(Translations), Name = "Longitude")]
        [AdditionalMetadata(AdditionalMetadataKeys.Mask, MaskPatterns.CoordinateDefault)]
        public decimal Longitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "BeginDateTime")]
        public DateTime BeginDateTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "EndDateTime")]
        public DateTime EndDateTime { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "City")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string City { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Province")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Province { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Zip")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Zip { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Street")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Street { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Number")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string Number { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "FloorAndDepartament")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public virtual string FloorAndDepartament { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Display(ResourceType = typeof(Translations), Name = "Organizer")]
        [Reference(SourceType = typeof(ListItemsHelper), SourceName = "GetEventAdministratorUsers")]
        public int Organizer { get; set; }

       
        [Display(ResourceType = typeof(Translations), Name = "Organizer")]
        [Editable(false)]
        public string OrganizerDisplay { get; set; }

        
       
    }
}