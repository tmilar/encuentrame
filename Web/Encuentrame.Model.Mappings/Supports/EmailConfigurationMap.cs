using Encuentrame.Model.Supports.EmailConfigurations;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Supports
{
    public class EmailConfigurationMap : MappingOf<EmailConfiguration>
    {
        public EmailConfigurationMap()
        {
            Map(x => x.Port);
            Map(x => x.EnableSsl);
            Map(x => x.FromEmail);
            Map(x => x.Host);
            Map(x => x.HostUser);
            Map(x => x.Password);
        }
    }
}
