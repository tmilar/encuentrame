using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Accounts
{
    public class ProfileModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        
        [Display(ResourceType = typeof(Translations), Name = "EmailAlternative")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        [UIHint("string")]
        public string EmailAlternative { get; set; }

        
        [Display(ResourceType = typeof(Translations), Name = "PhoneNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [UIHint("number")]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "MobileNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [UIHint("number")]
        public string MobileNumber { get; set; }

        


    }
}