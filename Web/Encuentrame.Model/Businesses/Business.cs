using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Addresses;
using Encuentrame.Support;

namespace Encuentrame.Model.Businesses
{
    public class Business:IDeleteable,IDisplayable
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime? DeletedKey { get; set; }

        public virtual string Name { get; set; }
        public virtual string Cuit { get; set; }
        public virtual DateTime Created { get; set; }

        private Address _address;

        public virtual Address Address
        {
            get => _address;
            set => _address = value ?? new Address();
        }

        private IList<User> _users;
        public virtual IList<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }

        public virtual string ToDisplay()
        {
            return Name;
        }
    }
}
