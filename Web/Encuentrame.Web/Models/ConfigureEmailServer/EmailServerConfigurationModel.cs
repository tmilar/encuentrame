using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Models.ConfigureEmailServer
{
    public class EmailServerConfigurationModel
    {
        [Display(ResourceType = typeof(Translations), Name = "Port")]        
        [UIHint("integer")]
        public virtual int Port { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Host")]       
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public virtual string Host { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EnableSsl")]
        [UIHint("bool")]
        public virtual bool EnableSsl { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "EmailFrom")]
        [EmailAddress(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailFromWrongFormat")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [UIHint("string")]
        public virtual string From { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "User")]
        public string User { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Password")]
        [UIHint("Password")]
        public string Password { get; set; }
    }
}