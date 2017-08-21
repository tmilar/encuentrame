using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models;
using Encuentrame.Web.Models.Accounts;
using Encuentrame.Web.Models.References;
using Encuentrame.Support;
using Encuentrame.Support.Email;
using Encuentrame.Support.Email.Templates;
using Encuentrame.Support.Email.Templates.EmailModels;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new []{ RoleEnum.Administrator})]
    public class ManageUserController : ListBaseController<User, UserListModel>
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
            var userParameters = Model.Accounts.UserCommand.CreateOrEditParameters.Instance();

            userParameters.Username = userModel.Username;
            userParameters.LastName = userModel.LastName;
            userParameters.FirstName = userModel.FirstName;
            userParameters.Email = userModel.Email;
            userParameters.EmailAlternative = userModel.EmailAlternative;
            userParameters.InternalNumber = userModel.InternalNumber;
            userParameters.PhoneNumber = userModel.PhoneNumber;
            userParameters.MobileNumber = userModel.MobileNumber;
            userParameters.Image = userModel.Image;
            userParameters.Role = userModel.Role;
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
            var userListModel = new UserListModel();
            userListModel.Id = user.Id;
            userListModel.User = user.Username;
            userListModel.FullName = user.FullName;
            userListModel.Email = user.Email;
            userListModel.InternalNumber = user.InternalNumber;
            userListModel.PhoneNumber = user.PhoneNumber;
            userListModel.Role = user.Role;
            return userListModel;
        }

        protected override IList<User> GetItemList()
        {
            return UserCommand.List();
        }        
    }
}