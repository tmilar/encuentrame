using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.SoughtPersons
{
    public class SoughtPerson
    {
        public virtual int UserId { get; protected set; }
        public double Distance { get; set; }
    }
}
