using System.Collections.Generic;
using System.Web.Mvc;
using Encuentrame.Model;
using NailsFramework.IoC;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Seekers;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.Accounts;
using Encuentrame.Support;
using Encuentrame.Support.Email;
using Encuentrame.Support.Email.Templates;
using Encuentrame.Support.Email.Templates.EmailModels;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new []{ RoleEnum.Administrator})]
    public class ManageAdministratorUserController : ListBaseController<User, UserListModel>
    {
        [Inject]
        public IUserCommand UserCommand { get; set; }

        [Inject]
        public IEmailService EmailService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var user = UserCommand.Get(id);

            var userModel = new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,
                Image = user.Image,
                Role = user.Role
            };

            return View(userModel);
        }

        public ActionResult Create()
        {
            var userModel = new UserModel();

            return View(userModel);
        }

        [HttpPost]
        public ActionResult Create(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var userParameters = GetCreateOrEditParameters(userModel);

                UserCommand.Create(userParameters);

                AddModelSuccess(Translations.CreateSuccess.FormatWith(TranslationsHelper.Get<User>()));

                if (!string.IsNullOrEmpty(userModel.Email))
                {
                    var welcomeUserEmailModel = new WelcomeUserEmailModel();
                    welcomeUserEmailModel.UserName = userModel.Username;
                    welcomeUserEmailModel.Site = "Encuentrame";
                    welcomeUserEmailModel.WelcomeInstructions = "A partir de ahora ud puede usar el sistema. Por favor ante cualquier duda o inconveniente comuniquese con el administrador.";

                    var header = new MailHeader();
                    header.To = userModel.Email;
                    header.Subject = "Bienvenido a Encuentrame";
                    EmailService.Send<WelcomeUserTemplate>(header, welcomeUserEmailModel);
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                return View(userModel);
            }
        }

        private UserCommand.CreateOrEditParameters GetCreateOrEditParameters(UserModel userModel)
        {
            var userParameters = new UserCommand.CreateOrEditParameters
            {
                Username = userModel.Username,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                Email = userModel.Email,
                EmailAlternative = userModel.EmailAlternative,
                InternalNumber = userModel.InternalNumber,
                PhoneNumber = userModel.PhoneNumber,
                MobileNumber = userModel.MobileNumber,
                Image = userModel.Image,
                Role = RoleEnum.Administrator
            };

            return userParameters;
        }

        public ActionResult Edit(int id)
        {
            var user = UserCommand.Get(id);

            var userModel = new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,
                Image = user.Image,
                Role = user.Role,
            };

            return View(userModel);
        }
        [HttpPost]
        public ActionResult Edit(int id, UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var userParameters = GetCreateOrEditParameters(userModel);
                UserCommand.Edit(id, userParameters);

                AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<User>()));
                return RedirectToAction("Index");
            }
            else
            {
                return View(userModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            UserCommand.Delete(id);

            return RedirectToAction("Index");
        }

        protected override UserListModel GetViewModelFrom(User user)
        {
            var userListModel = new UserListModel
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
            return userListModel;
        }

        protected override void ApplyDefaultFilters(IGenericSeeker<User> seeker)
        {
            base.ApplyDefaultFilters(seeker);
            ((IUserSeeker) seeker).ByRole(RoleEnum.Administrator);
        }
    }
}