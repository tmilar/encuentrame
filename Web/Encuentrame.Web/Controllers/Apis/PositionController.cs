using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Encuentrame.Security.Authentications;
using Encuentrame.Support;
using Encuentrame.Web.Models.Apis.Authentications;
using Encuentrame.Web.Models.Apis.Positions;
using NailsFramework.UserInterface;

namespace Encuentrame.Web.Controllers.Apis
{
    public class PositionController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Set(PositionApiModel positionApiModel)
        {
            if (positionApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new PositionResultModel()
            {
               
            });

        }
    }
}