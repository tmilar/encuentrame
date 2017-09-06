using System.ComponentModel.DataAnnotations;
using Encuentrame.Model.Accounts;

namespace Encuentrame.Web.Models.Accounts
{
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Username")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Username { get; set; }
       
        [Display(ResourceType = typeof(Translations), Name = "LastName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public string LastName { get; set; }
        
        [Display(ResourceType = typeof(Translations), Name = "FirstName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Email")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        [UIHint("string")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EmailAlternative")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailError")]
        [UIHint("string")]
        public string EmailAlternative { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "InternalNumber")]
        [StringLength(10, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [UIHint("number")]
        public string InternalNumber { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "PhoneNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [UIHint("number")]
        public string PhoneNumber { get; set; }
        
        [Display(ResourceType = typeof(Translations), Name = "MobileNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [UIHint("number")]
        public string MobileNumber { get; set; }
        
        [Display(ResourceType = typeof(Translations), Name = "Image")]
        [UIHint("image")]
        public virtual string Image { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "PhoneNumber")]
        
        [UIHint("Enum")]
        public RoleEnum Role { get; set; }

    }
}