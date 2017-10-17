using System.Web.Mvc;
using Encuentrame.Web.Helpers;

namespace Encuentrame.Web.Controllers
{
    public class ReferenceItemsController : BaseController
    {
       
       
      

        [HttpGet]
        public ActionResult GetUsers()
        {
            var result = ListItemsHelper.GetUsersList();

            return Json(JsReturnHelper.Return(result), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEventAdministratorUsers()
        {
            var result = ListItemsHelper.GetEventAdministratorUsersList();

            return Json(JsReturnHelper.Return(result), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEntities()
        {
            var result = ListItemsHelper.GetEntitiesList();

            return Json(JsReturnHelper.Return(result), JsonRequestBehavior.AllowGet);
        }

       
    }
}