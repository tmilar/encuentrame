using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Support.Email;

namespace Encuentrame.Model.Supports.EmailConfigurations
{
    [Lemming]
    public class EmailConfigurationCommand : BaseCommand, IEmailConfigurationCommand
    {
        [Inject]
        public IBag<EmailConfiguration> EmailConfigurations { get; set; }

        [Inject]
        public IEmailService EmailService { get; set; }

        public EmailConfiguration Get()
        {
            return EmailConfigurations.FirstOrDefault();
        }

        public void Save(CreateOrEditParameters parameters)
        {
            var configuration = EmailConfigurations.FirstOrDefault();
            if (configuration == null)
            {
                configuration = new EmailConfiguration();
                UpdateConfiguration(configuration, parameters);
                EmailConfigurations.Put(configuration);
            }
            else
            {
                UpdateConfiguration(configuration, parameters);
            }
            var emailServerConfiguration = new EmailServerConfiguration();
            emailServerConfiguration.Port = configuration.Port;
            emailServerConfiguration.EnableSsl = configuration.EnableSsl;
            emailServerConfiguration.User = configuration.HostUser;
            emailServerConfiguration.Password = configuration.Password;
            emailServerConfiguration.Host = configuration.Host;
            emailServerConfiguration.From = configuration.FromEmail;

            EmailService.Reset(emailServerConfiguration);
        }

        private void UpdateConfiguration(EmailConfiguration configuration, CreateOrEditParameters emailServerConfigurationModel)
        {
            configuration.EnableSsl = emailServerConfigurationModel.EnableSsl;
            configuration.Port = emailServerConfigurationModel.Port;
            configuration.Host = emailServerConfigurationModel.Host;
            configuration.FromEmail = emailServerConfigurationModel.From;
            configuration.Password = emailServerConfigurationModel.Password;
            configuration.HostUser = emailServerConfigurationModel.User;
        }

        public class CreateOrEditParameters
        {
            protected CreateOrEditParameters()
            {
            }

            public static CreateOrEditParameters Instance()
            {
                return new CreateOrEditParameters();
            }

            public virtual string Host { get; set; }
            public virtual string User { get; set; }
            public virtual string Password { get; set; }
            public virtual bool EnableSsl { get; set; }
            public virtual string From { get; set; }
            public virtual int Port { get; set; }
        }
    }
}