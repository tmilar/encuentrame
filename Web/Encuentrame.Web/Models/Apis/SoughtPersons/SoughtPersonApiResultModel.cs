using Encuentrame.Web.Models.Apis.Accounts;

namespace Encuentrame.Web.Models.Apis.SoughtPersons
{
    public class SoughtPersonApiResultModel
    {
        public UserApiResultModel User { get; set; }
        public int Distance { get; set; }
    }
}