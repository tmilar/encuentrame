using System;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.AreYouOks
{
    public class AreYouOk : IIdentifiable
    {
        public AreYouOk()
        {
            Created = SystemDateTime.Now;
        }
        public virtual int Id { get; protected set; }
        public virtual User Sender { get; set; }
        public virtual User Target { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime? ReplyDatetime { get; set; }
        public virtual bool IAmOk { get; set; }
    }
}
