using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Authentications;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UserInterface;

namespace Encuentrame.Web.Controllers.Apis
{
    public class AuthenticationController : BaseApiController
    {
        [Inject]
        public IAuthenticationProvider AuthenticationProvider { get; set; }

      
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login(LoginApiModel loginApiModel)
        {

            if (loginApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            if (AuthenticationProvider.ValidateUser(loginApiModel.Username, loginApiModel.Password))
            {
                var token = AuthenticationProvider.GenerateApiTokenUser(loginApiModel.Username);
                var result=new LoginApiResultModel()
                {
                    Token = token.Token,
                    UserId = token.UserId
                };
                return Ok(result);
            }
            return Unauthorized();

        }

       


    }
}
