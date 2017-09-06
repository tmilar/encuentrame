using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Web.Models.Apis.Authentications;
using NailsFramework.IoC;

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
         
            return Content(HttpStatusCode.Unauthorized, new {Message= "Invalid user name or password" }); ;

        }

       


    }
}
