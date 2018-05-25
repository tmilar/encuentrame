using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Web.Models.Apis.Authentications;

namespace Encuentrame.Web.Controllers
{
    [AllowAnonymous]
    public class TestApiController : BaseController
    {
        // GET: TestApi
        public ActionResult Index()
        {
            return View();
        }
    }

  
}