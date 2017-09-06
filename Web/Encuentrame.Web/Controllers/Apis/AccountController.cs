using System.Web.Http;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Accounts;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class AccountController : BaseApiController
    {
        [Inject]
        public IUserCommand UserCommand { get; set; }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Create(UserApiModel userApiModel)
        {

            if (userApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userParameters = new UserCommand.CreateOrEditParameters
            {
                Username = userApiModel.Username,
                LastName = userApiModel.LastName,
                FirstName = userApiModel.FirstName,
                Email = userApiModel.Email,
                EmailAlternative = userApiModel.EmailAlternative,
                InternalNumber = userApiModel.InternalNumber,
                PhoneNumber = userApiModel.PhoneNumber,
                MobileNumber = userApiModel.MobileNumber,
                Image = userApiModel.Image.IsNullOrEmpty() ? string.Empty : "data:image/jpeg;base64," + userApiModel.Image,
                Role = RoleEnum.User
            };

            UserCommand.NewRegister(userParameters);

            return Ok();

        }

        [HttpPost]
        public IHttpActionResult Update(UserApiModel userApiModel)
        {

            if (userApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userParameters = new UserCommand.CreateOrEditParameters
            {
                Username = userApiModel.Username,
                LastName = userApiModel.LastName,
                FirstName = userApiModel.FirstName,
                Email = userApiModel.Email,
                EmailAlternative = userApiModel.EmailAlternative,
                InternalNumber = userApiModel.InternalNumber,
                PhoneNumber = userApiModel.PhoneNumber,
                MobileNumber = userApiModel.MobileNumber,
                Image = userApiModel.Image.IsNullOrEmpty() ? string.Empty : "data:image/jpeg;base64," + userApiModel.Image,
            };

            UserCommand.EditRegister(userParameters);

            return Ok();


        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var user = UserCommand.Get(GetIdUserLogged());

            var userApiModel = new UserApiModel()
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,
                Image = user.Image,
            };

            return Ok(userApiModel);


        }
    }
}