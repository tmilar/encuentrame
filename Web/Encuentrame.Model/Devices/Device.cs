using System;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.Devices
{
    public class Device:IIdentifiable
    {
        public Device()
        {
            Created = SystemDateTime.Now;
        }
        public virtual int Id { get; protected set; }
        public virtual User User { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime Created { get; set; }
    }
}
