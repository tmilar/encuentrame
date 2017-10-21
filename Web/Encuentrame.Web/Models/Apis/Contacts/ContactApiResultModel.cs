using Encuentrame.Web.Models.Apis.Accounts;

namespace Encuentrame.Web.Models.Apis.Contacts
{
    public class ContactApiResultModel
    {
        public UserApiResultModel User { get; set; }
        public bool Pending { get; set; }
    }
}