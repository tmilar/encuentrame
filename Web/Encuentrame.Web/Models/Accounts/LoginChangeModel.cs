using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Accounts
{
    public class LoginChangeModel
    {
        [Display(ResourceType = typeof(Translations), Name = "OldPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [UIHint("password")]
        public string OldPassword { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "NewPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [UIHint("password")]
        public string NewPassword { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "RepeatPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Compare("NewPassword")]
        [UIHint("password")]
        public string RepeatPassword { get; set; }


    }
}