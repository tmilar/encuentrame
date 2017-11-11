using System.Web.Mvc;
using System.Web.Security;
using NailsFramework.IoC;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Web.Controllers
{
    public class AccountController : BaseController
    {
        [Inject]
        public IAuthenticationProvider Authentication { get; set; }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var loginModel = new LoginModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Authentication.ValidateUser(loginModel.Username, loginModel.Password, RoleEnum.Administrator, RoleEnum.EventAdministrator))
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, true);
                    if (returnUrl.IsNullOrEmpty())
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    AddModelError(Translations.LoginFailError);
                }
            }
            return View(loginModel);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            var user = Authentication.GetLoggedUser();

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
            };

            return View(userModel);
            
        }

        [HttpGet]
        public ActionResult ProfileEdit()
        {
            var user = Authentication.GetLoggedUser();

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
            };

            return View(userModel);
        }

        [HttpPost]
        public ActionResult ProfileEdit(ProfileModel profileModel)
        {
            var user = Authentication.GetLoggedUser();
            if (ModelState.IsValid)
            {
                user.EmailAlternative = profileModel.EmailAlternative;
                
                user.MobileNumber = profileModel.MobileNumber;
                user.PhoneNumber = profileModel.PhoneNumber;
                
                AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<User>()));
                return RedirectToAction("UserProfile");
            }
            else
            {
                var userModel = new UserModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    EmailAlternative = profileModel.EmailAlternative,
                    InternalNumber = user.InternalNumber,
                    PhoneNumber = profileModel.PhoneNumber,
                    MobileNumber = profileModel.MobileNumber,
                    Image = user.Image,
                };
                return View(userModel);
            }
            
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var loginChangeModel = new LoginChangeModel();
            return View(loginChangeModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(LoginChangeModel loginChangeModel)
        {
            if (ModelState.IsValid)
            {
                if (Authentication.ChangePassword(loginChangeModel.OldPassword, loginChangeModel.NewPassword))
                {
                    AddModelSuccess(Translations.ChangePasswordSuccess);
                    return RedirectToAction("UserProfile", "Account");
                }
                else
                {
                    AddModelError(Translations.ChangePasswordFailError);
                }
            }
            return View(loginChangeModel);
        }
    }
}