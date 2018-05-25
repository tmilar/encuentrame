using System;
using System.Collections.Concurrent;
using NailsFramework.IoC;
using Encuentrame.Support.Email.Templates;

namespace Encuentrame.Support.Email
{
    [Lemming]
    public class EmailTemplateManager : IEmailTemplateManager
    {
        public const string WelcomeUserTemplateName = "WelcomeUser";
        public const string ObjectCreatedTemplateName = "ObjectCreated";
        public const string ObjectUpdatedTemplateName = "ObjectUpdated";
        public const string ObjectDeletedTemplateName = "ObjectDeleted";

        private static readonly ConcurrentDictionary<string, Func<dynamic, BaseTemplate>> Templates;        

        static EmailTemplateManager()
        {
            Templates = new ConcurrentDictionary<string, Func<dynamic, BaseTemplate>>();
            Templates.AddOrUpdate(WelcomeUserTemplateName, (model) => GetTemplate<WelcomeUserTemplate>(model), (key, oldvalue) =>
            {
                return (x) =>
                {
                    var template1 = GetTemplate<WelcomeUserTemplate>(x);
                    return template1;

                };
            });

            Templates.AddOrUpdate(ObjectCreatedTemplateName, (model) => GetTemplate<ObjectCreatedTemplate>(model), (key, oldvalue) =>
            {
                return (x) =>
                {
                    var template1 = GetTemplate<ObjectCreatedTemplate>(x);
                    return template1;

                };
            });

            Templates.AddOrUpdate(ObjectUpdatedTemplateName, (model) => GetTemplate<ObjectUpdatedTemplate>(model), (key, oldvalue) =>
            {
                return (x) =>
                {
                    var template1 = GetTemplate<ObjectUpdatedTemplate>(x);
                    return template1;

                };
            });

            Templates.AddOrUpdate(ObjectDeletedTemplateName, (model) => GetTemplate<ObjectDeletedTemplate>(model), (key, oldvalue) =>
            {
                return (x) =>
                {
                    var template1 = GetTemplate<ObjectDeletedTemplate>(x);
                    return template1;

                };
            });
        }        

        private static BaseTemplate GetTemplate<T>(dynamic model) where T : BaseTemplate, new()
        {
            var template1 = new T {Model = model};
            return template1;
        }

        public string GetEmailBody(string templateName, dynamic model)
        {
            var template = Templates[templateName](model);
            return template.TransformText();
        }

        public string GetEmailBody<T>(dynamic model) where T: BaseTemplate, new()
        {
            var template = GetTemplate<T>(model);
            return template.TransformText();
        }

        public void Register<T>(string templateName) where T : BaseTemplate, new()
        {
            Templates.AddOrUpdate(templateName, (model) => GetTemplate<T>(model), (key, oldvalue) =>
            {
                return (x) =>
                {
                    var template1 = GetTemplate<T>(x);
                    return template1;

                };
            });
        }
    }
}