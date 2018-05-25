using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.MetadataProviders;
using Encuentrame.Web.MetadataProviders.ConditionalValidations;
using Encuentrame.Web.MetadataProviders.CustomValidations;

namespace Encuentrame.Web.Models.Notifications
{
    public class NotificationAccessInfo
    {
        public int Id { get; set; }
        public string NotificationName { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Roles")]
        [ReferenceMultiple(SourceType = typeof(ListItemsHelper), SourceName = "GetRolesSelectList")]
        public IList<int> Roles { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Users")]
        [ReferenceMultiple(SourceType = typeof(ListItemsHelper), SourceName = "GetUserSelectListItems")]
        public IList<int> Users { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "AllowEveryone")]
        [UIHint("bool")]
        public bool AllowEveryone { get; set; }
    }

    public class NotificationsConfigurationModel
    {
        public IList<NotificationAccessInfo> NotificationAccessInfos { get; set; }

        public NotificationsConfigurationModel()
        {
            NotificationAccessInfos = new List<NotificationAccessInfo>();
        }
    }
}