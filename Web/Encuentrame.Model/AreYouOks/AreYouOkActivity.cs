using Encuentrame.Model.Accounts;

namespace Encuentrame.Model.AreYouOks
{
    public class AreYouOkActivity : BaseAreYouOk
    {
        public virtual User Sender { get; set; }
    }
}