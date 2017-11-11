using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Accounts;
using Encuentrame.Web.Models.Devices;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers.Apis
{
    public class AccountController : BaseApiController
    {
        [Inject]
        public IUserCommand UserCommand { get; set; }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
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
                Password = userApiModel.Password,
                Lastname = userApiModel.Lastname,
                Firstname = userApiModel.Firstname,
                Email = userApiModel.Email,
                EmailAlternative = userApiModel.EmailAlternative,
                InternalNumber = userApiModel.InternalNumber,
                PhoneNumber = userApiModel.PhoneNumber,
                MobileNumber = userApiModel.MobileNumber,
                Role = RoleEnum.User
            };

            try
            {
                UserCommand.NewRegister(userParameters);

                return Ok();
            }
            catch (UserUsernameUniqueException e)
            {
                ModelState.AddModelError("UserApiModel.Username", Translations.UserUsernameUniqueException);
                return BadRequest(ModelState);
            }



        }

        [System.Web.Http.HttpPost]
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
                Lastname = userApiModel.Lastname,
                Firstname = userApiModel.Firstname,
                Email = userApiModel.Email,
                EmailAlternative = userApiModel.EmailAlternative,
                InternalNumber = userApiModel.InternalNumber,
                PhoneNumber = userApiModel.PhoneNumber,
                MobileNumber = userApiModel.MobileNumber,
            };

            UserCommand.EditRegister(userParameters);

            return Ok();


        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Devices(DeviceApiModel deviceApiModel)
        {
            var deviceParameters = new UserCommand.DeviceParameters
            {
                UserId = this.GetIdUserLogged(),
                Token = deviceApiModel.Token,
            };

            UserCommand.SetDevice(deviceParameters);

            return Ok();
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            var user = UserCommand.Get(GetIdUserLogged());

            var userApiModel = new UserApiModel()
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailAlternative = user.EmailAlternative,
                InternalNumber = user.InternalNumber,
                PhoneNumber = user.PhoneNumber,
                MobileNumber = user.MobileNumber,

            };

            return Ok(userApiModel);
        }


        [System.Web.Http.HttpGet]
        public IHttpActionResult GetAll()
        {
            var users = UserCommand.ListUsers();
            var list = new List<UserApiResultModel>();
            foreach (var user in users)
            {
                var userApiModel = new UserApiResultModel()
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
                };

                list.Add(userApiModel);
            }



            return Ok(list);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage GetImage(int id)
        {
            try
            {
                var user = UserCommand.Get(id);
                MemoryStream img = user.Image.Base64ToImageMemoryStreamMemory();
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(img.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }
            catch (Exception )
            {

                MemoryStream img = ImageExtensions.ImageMemoryStreamFromContentFolder("no-image.png");
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(img.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return result;
            }
           


        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult UploadImage()
        {
            var user = UserCommand.Get(this.GetIdUserLogged());

            foreach (string file in HttpContext.Current.Request.Files)
            {
                var imageFile = HttpContext.Current.Request.Files[file];
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var isValid = imageFile.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                                  imageFile.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                                  imageFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
                                  imageFile.FileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase);
                    if (isValid)
                    {
                        var imageBase64 = imageFile.InputStream.ImageToBase64(500, 0, PixelFormat.Format32bppPArgb);
                        if (imageBase64.NotIsNullOrEmpty())
                        {
                            user.Image = imageBase64;
                            return Ok();
                        }
                    }
                }
            }
            return BadRequest();
        }
    }
}