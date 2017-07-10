using System.ComponentModel.DataAnnotations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.MetadataProviders;

namespace Encuentrame.Web.Models.Accounts
{
    public class RoleModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Name")]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "MaxLengthError")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Name { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "Passes")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Tree(SourceType = typeof(ListTreeItemsHelper), SourceName = "GetPasses")]
        public string Passes { get; set; }

    }
}