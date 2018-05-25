using Encuentrame.Support;

namespace Encuentrame.Model.Supports.EmailConfigurations
{
    public class EmailConfiguration : IIdentifiable
    {
        public virtual int Port { get; set; }
        public virtual string Host { get; set; }
        public virtual bool EnableSsl { get; set; }
        public virtual string FromEmail { get; set; }
        public virtual int Id { get; protected set; }
        public virtual string HostUser { get; set; }
        public virtual string Password { get; set; }
    }
}
