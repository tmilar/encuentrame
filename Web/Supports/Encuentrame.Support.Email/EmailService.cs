using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NailsFramework.IoC;
using Encuentrame.Support.Email.Templates;

namespace Encuentrame.Support.Email
{
    [Lemming]
    public class EmailService : IEmailService
    {
        private bool _initialized = false;
        private EmailServerConfiguration _configuration;

        private bool On
        {
            get
            {
                return Configuration != null && (!string.IsNullOrEmpty(Configuration.Host) || !string.IsNullOrEmpty(EmailSavePath));
            }
        }

        public EmailServerConfiguration Configuration
        {
            get
            {
                if (!_initialized && GetConfiguration != null && _configuration==null)
                {
                    _configuration = GetConfiguration();
                    _initialized = true;
                }
                    
                return _configuration;
            }
            set
            {
                _configuration = value;
                if (_configuration != null)
                    _initialized = true;
            }
        }

        public Func<EmailServerConfiguration> GetConfiguration { get; set; }

        [Inject]
        public IEmailTemplateManager EmailTemplateManager { get; set; }

        [Inject("EmailSavePath")]
        public static string EmailSavePath { get; set; }

        public EmailService()
        {            
        }

        public void Init(EmailServerConfiguration configuration)
        {
            this.Configuration = configuration;         
        }

        public void Reset(EmailServerConfiguration configuration)
        {
            Init(configuration);
        }

        public void Send(MailHeader header, string template, object model)
        {
            var mailMessage = GetMailMessage(header, template, model);
            Send(mailMessage);
        }

        public void Send<T>(MailHeader header, object model) where T: BaseTemplate, new()
        {
            var mailMessage = GetMailMessage<T>(header, model);
            Send(mailMessage);
        }

        public void Send(IList<MailHeader> headers, string template, dynamic model)
        {
            foreach (var mailHeader in headers)
            {
                var mailMessage = GetMailMessage(mailHeader, template, model);
                Send(mailMessage);   
            }            
        }

        private MailMessage GetMailMessage(MailHeader header, string body)
        {
            var configuration = Configuration;
            if (configuration == null)
                return null;
            
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(configuration.From),
                Subject = header.Subject,
                IsBodyHtml = header.IsHtml
            };

            //to
            mailMessage.To.Add(header.To);

            //cc
            if (!string.IsNullOrEmpty(header.Cc))
            {
                mailMessage.CC.Add(header.Cc);
            }

            //bcc
            if (!string.IsNullOrEmpty(header.Bcc))
            {
                mailMessage.Bcc.Add(header.Bcc);
            }

            mailMessage.Body = body;

            return mailMessage;
        }

        private MailMessage GetMailMessage<T>(MailHeader header, dynamic model) where T: BaseTemplate, new()
        {            
            var body = EmailTemplateManager.GetEmailBody<T>(model);

            return GetMailMessage(header, body);
        }

        private MailMessage GetMailMessage(MailHeader header, string template, dynamic model)
        {
            var body = EmailTemplateManager.GetEmailBody(template, model);
            return GetMailMessage(header, body);
        }

        private void Send(MailMessage message)
        {
            if (On)
            {
                SmtpClient smtpClient = GetSmtpClient(); //SmtpClient configuration out of this scope
                //MailMessage message = new MailMessage(); //MailMessage configuration out of this scope

                //smtpClient.SendCompleted += (s, e) =>
                //{
                //    SmtpClient callbackClient = s as SmtpClient;
                //    MailMessage callbackMailMessage = e.UserState as MailMessage;
                //    if (callbackClient != null) callbackClient.Dispose();
                //    if (callbackMailMessage != null) callbackMailMessage.Dispose();
                //};

                //smtpClient.SendAsync(message, message);
                smtpClient.Send(message); 
            }            
        }

        private SmtpClient GetSmtpClient()
        {            
            var configuration = this.Configuration;
            if (!string.IsNullOrEmpty(EmailSavePath))
            {
                var smtpClientTest = new SmtpClient();
                smtpClientTest.PickupDirectoryLocation = EmailSavePath;
                smtpClientTest.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                return smtpClientTest;
            }
            var smtpClient = new SmtpClient(configuration.Host);
            if(configuration.Port!=default(int))
                smtpClient.Port = configuration.Port;

            if (!string.IsNullOrEmpty(configuration.User))
                smtpClient.Credentials = new System.Net.NetworkCredential(configuration.User, configuration.Password);

            smtpClient.EnableSsl = configuration.EnableSsl;
            return smtpClient;
        }
    }
}
