using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Web.Helpers;

namespace Encuentrame.Web.Models.Audits
{
    public class AuditListModel
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public ActionsEnum AuditBehaviorType { get; set; }
        public ItemModel User { get; set; }
        public EntityTypeModel EntityType { get; set; }
        public DateTime Date { get; set; }
        public IList<AdditionalDataModel> AdditionalDataModels { get; set; }
    }
}