using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Accounts
{
    public class LoginModel
    {

        [Display(ResourceType = typeof(Translations), Name = "Username")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Username { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "Password")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [UIHint("password")]
        public string Password { get; set; }
       
       
    }
}