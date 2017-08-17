using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Model.Supports;
using Encuentrame.Model.Supports.EmailConfigurations;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.ConfigureEmailServer;
using Encuentrame.Support;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new []{RoleEnum.Administrator, })]
    public class ConfigureEmailServerController: BaseController
    {
        [Inject]
        public IBag<EmailConfiguration> EmailConfigurations { get; set; }

        [Inject]
        public IEmailConfigurationCommand EmailConfigurationCommand { get; set; }

        public ActionResult Index()
        {
            var viewModel = GetEmailServerConfiguration();
            return View(viewModel);
        }

        private EmailServerConfigurationModel GetEmailServerConfiguration()
        {
            var configuration = EmailConfigurationCommand.Get();
            var viewModel = new EmailServerConfigurationModel();
            if (configuration != null)
            {
                viewModel.Host = configuration.Host;
                viewModel.From = configuration.FromEmail;
                viewModel.EnableSsl = configuration.EnableSsl;
                viewModel.Port = configuration.Port;
                viewModel.User = configuration.HostUser;
                viewModel.Password = configuration.Password;
            }
            return viewModel;
        }

        public ActionResult Save(EmailServerConfigurationModel emailServerConfigurationModel)
        {
            var parameters = Model.Supports.EmailConfigurations.EmailConfigurationCommand.CreateOrEditParameters.Instance();

            parameters.EnableSsl = emailServerConfigurationModel.EnableSsl;
            parameters.Port = emailServerConfigurationModel.Port;
            parameters.Host = emailServerConfigurationModel.Host;
            parameters.From = emailServerConfigurationModel.From;
            parameters.Password = emailServerConfigurationModel.Password;
            parameters.User = emailServerConfigurationModel.User;

            EmailConfigurationCommand.Save(parameters);
            AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<EmailConfiguration>()));
            return Redirect("Index");
        }

        
    }
}