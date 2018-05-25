using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NailsFramework;
using NailsFramework.Config;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using NailsFramework.UserInterface;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Model.Supports.EmailConfigurations;
using Encuentrame.Security.Authentications;
using Encuentrame.Web.MetadataProviders;
using Encuentrame.Web.MetadataProviders.CustomValidations;
using Encuentrame.Web.Models.References.Commons;
using Encuentrame.Web.Support;
using Encuentrame.Support;
using Encuentrame.Support.Email;
using Encuentrame.Support.Mappings;
using ILog = NailsFramework.Logging.ILog;

namespace Encuentrame.Web
{
    public class MvcApplication : NailsMvcApplication
    {
        protected void Application_Start()
        {
            RegisterValidatorAdapters();
            RegisterModelBinders();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelMetadataProviders.Current = new ExtendedMetadataProvider();
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "MvcErrorMessages";
            DefaultModelBinder.ResourceClassKey = "MvcErrorMessages";

            log4net.GlobalContext.Properties["ip"] = Dns.GetHostAddresses(Dns.GetHostName()).Join(";", x => x.MapToIPv4().ToString());
        }

        protected override void PostNailsInitialize()
        {
            base.PostNailsInitialize();

            var emailService = Nails.ObjectFactory.GetObject<IEmailService>();
            emailService.GetConfiguration = () =>
            {
                var emailConfigurations = Nails.ObjectFactory.GetObject<IBag<EmailConfiguration>>();

                var configuration = emailConfigurations.FirstOrDefault();
                if (configuration != null)
                {
                    var emailServerConfiguration = new EmailServerConfiguration();
                    emailServerConfiguration.Host = configuration.Host;
                    emailServerConfiguration.From = configuration.FromEmail;
                    emailServerConfiguration.User = configuration.HostUser;
                    emailServerConfiguration.Password = configuration.Password;
                    emailServerConfiguration.Port = configuration.Port;
                    emailServerConfiguration.EnableSsl = configuration.EnableSsl;

                    return emailServerConfiguration;
                }
                return null;
            };
        }

        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(ReferenceAny), new ReferenceAnyModelBinder());
            ModelBinders.Binders.Add(typeof(IEnumerable<ReferenceAny>), new ReferenceAnyListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<ReferenceAny>), new ReferenceAnyListModelBinder());
            ModelBinders.Binders.Add(typeof(List<ReferenceAny>), new ReferenceAnyListModelBinder());
        }

        private void RegisterValidatorAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredListAttribute),
                typeof(RequiredAttributeAdapter)
            );

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredReference2Attribute),
                typeof(RequiredAttributeAdapter)
            );

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredReferenceAny2Attribute),
                typeof(RequiredAttributeAdapter)
            );
        }

        protected void Application_Error()
        {
            if (Context.IsCustomErrorEnabled)
            {
                var ex = Server.GetLastError();
                Nails.ObjectFactory.GetObject<ILog>().Error(ex.Message, ex);
                Server.ClearError();

                Response.Redirect("~/Home/Index");
            }

        }

        protected override void ConfigureNails(INailsConfigurator nails)
        {
            nails.IoC.Container<NailsFramework.IoC.Spring>()
                .Persistence.DataMapper<NailsFramework.Persistence.NHibernate>(
                    x => x.Configure(c => MappingConfigurator.Configure(c)))
                .UserInterface.Platform<Mvc>()
                .Logging.Logger<Log4net>()
                .InspectAssembly(@"bin\Encuentrame.Model.dll")
                .InspectAssembly(@"bin\Encuentrame.Support.Mappings.dll")
                .InspectAssembly(@"bin\Encuentrame.Model.Mappings.dll")
                .InspectAssembly(@"bin\Encuentrame.Security.dll")
                .InspectAssembly(@"bin\Encuentrame.Web.dll")
                .InspectAssembly(@"bin\Encuentrame.Support.Email.dll")
                .InspectAssembly(@"bin\Encuentrame.Support.ExpoNotification.dll")
                .Aspects
                .ApplyBehavior<LogBehavior>().ToInheritorsOf(typeof(ControllerBase))
                .ApplyBehavior<AuditBehavior>().ToMethodsWithAttribute<AuditAttribute>()
                .IoC.Lemming<DomainAuthenticationProvider>()
                .IoC.Lemming<AuthenticationDataProvider>()
                .IoC.Lemming<TranslationService>();
        }
    }
}
