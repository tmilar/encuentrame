using System;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.Contacts
{
    public class Contact:IIdentifiable
    {
        public Contact()
        {
            Created = SystemDateTime.Now;
        }
        public virtual int Id { get; protected set; }

        public virtual User User { get; set; }

        public virtual ContactRequestStatus Status { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime? AcceptedDatetime { get; set; }
    }
}
