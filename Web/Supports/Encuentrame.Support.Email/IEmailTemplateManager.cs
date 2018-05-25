using Encuentrame.Support.Email.Templates;

namespace Encuentrame.Support.Email
{
    public interface IEmailTemplateManager
    {
        string GetEmailBody(string templateName, dynamic model);
        string GetEmailBody<T>(dynamic model) where T : BaseTemplate, new();
        void Register<T>(string templateName) where T : BaseTemplate, new();
    }
}