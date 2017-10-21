using Encuentrame.Model.Events;

namespace Encuentrame.Model.AreYouOks
{
    public class AreYouOkEvent : BaseAreYouOk
    {
        public virtual Event Event { get; set; }
    }
}