using System;
using System.Collections.Generic;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Model.Accounts
{
    public class Role : IDeleteable, IDisplayable
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        private IList<Pass> _passes;
        public virtual IList<Pass> Passes
        {
            get { return _passes ?? (_passes = new List<Pass>()); }
            set { _passes = value; }
        }
        public virtual DateTime? DeletedKey { get; set; }
        public virtual string ToDisplay()
        {
            return Name;
        }
    }
}