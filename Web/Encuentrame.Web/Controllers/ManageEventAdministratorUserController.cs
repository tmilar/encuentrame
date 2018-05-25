using System.Web.Mvc;
using Encuentrame.Model;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Seekers;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;
using Encuentrame.Support.Email;
using Encuentrame.Support.Email.Templates;
using Encuentrame.Support.Email.Templates.EmailModels;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models;
using Encuentrame.Web.Models.Accounts;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator,RoleEnum.EventAdministrator })]
    public class ManageEventAdministratorUserController : ListBaseController<User, UserListModel>
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
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,
                Image = user.Image,
                Role = user.Role,
                BusinessDisplay = user.Business.ToDisplay(),
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
                    welcomeUserEmailModel.Username = userModel.Username;
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
                Lastname = userModel.Lastname,
                Firstname = userModel.Firstname,
                Email = userModel.Email,
                EmailAlternative = userModel.EmailAlternative,
                InternalNumber = userModel.InternalNumber,
                PhoneNumber = userModel.PhoneNumber,
                MobileNumber = userModel.MobileNumber,
                Image = userModel.Image.RemoveBase64Prefix(),
                Role = RoleEnum.EventAdministrator,
                
            };

            var loggedUser = this.GetLoggedUser();
            if (loggedUser.Role == RoleEnum.EventAdministrator)
            {
                userParameters.Business = loggedUser.Business.Id;
            }
            else
            {
                userParameters.Business = userModel.Business;
            }
           

            return userParameters;
        }

        public ActionResult Edit(int id)
        {
            var user = UserCommand.Get(id);

            var userModel = new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,
                Image = user.Image,
                Role = user.Role,
                Business = user.Business.Id
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
                Role = user.Role,
                Business = new ItemModel() { Id = user.Business.Id,Name = user.Business.ToDisplay()}
            };
            return userListModel;
        }

        protected override void ApplyDefaultFilters(IGenericSeeker<User> seeker)
        {
            base.ApplyDefaultFilters(seeker);
            ((IUserSeeker)seeker).ByRole(RoleEnum.EventAdministrator);

            var loggedUser = this.GetLoggedUser();
            if (loggedUser.Role == RoleEnum.EventAdministrator)
            {
                ((IUserSeeker) seeker).ByBusiness(loggedUser.Business.Id);
            }
        }
    }
}