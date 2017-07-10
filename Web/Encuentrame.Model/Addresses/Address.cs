using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Support;

namespace Encuentrame.Model.Addresses
{
    public abstract class Address: IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual string City { get; set; }
        public virtual string Province { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Street { get; set; }
        public virtual string Number { get; set; }
        public virtual string FloorAndDepartament { get; set; }
        public virtual bool IsDefault { get; set; }
    }

    public class ClientAddress : Address
    {
        
    }
}
