using System;
using System.Collections.Generic;
using Encuentrame.Support.Email.Templates;

namespace Encuentrame.Support.Email
{
    public interface IEmailService
    {
        void Init(EmailServerConfiguration configuration);
        void Reset(EmailServerConfiguration configuration);
        void Send(MailHeader header, string template, object model);
        void Send<T>(MailHeader header, object model) where T : BaseTemplate, new();
        void Send(IList<MailHeader> headers, string template, dynamic model);
        Func<EmailServerConfiguration> GetConfiguration { get; set; }
    }
}