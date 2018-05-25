using System.Web.Mvc;
using Encuentrame.Model;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Seekers;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support.Email;
using Encuentrame.Web.Models.Accounts;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator })]
    public class ManagePersonUserController : ListBaseController<User, UserListModel>
    {
        [Inject]
        public IUserCommand UserCommand { get; set; }

       

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
                Role = user.Role
            };

            return View(userModel);
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
            ((IUserSeeker)seeker).ByRole(RoleEnum.User);
        }
    }
}