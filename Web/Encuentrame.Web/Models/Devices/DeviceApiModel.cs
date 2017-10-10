using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.Models.Devices
{
   
    public class DeviceApiModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string Token { get; set; }
    }
}