using System;
using Encuentrame.Support;

namespace Encuentrame.Model.Accounts
{
    public class TokenApiSession:IIdentifiable
    {
        public virtual int  Id { get; protected set; }
        public virtual string Token { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime ExpiredDateTime { get; set; }
    }
}