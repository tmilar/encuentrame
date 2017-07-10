using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Supports;
using Encuentrame.Support;

namespace Encuentrame.Web.Controllers
{
    public class ResourcesController : BaseController
    {
        // GET: Resources
        [HttpPost]
        public JsonResult UploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var imageFile = System.Web.HttpContext.Current.Request.Files["image"];
                var isValid = imageFile.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) || imageFile.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || imageFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || imageFile.FileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase);
                var imageBase64 = imageFile.InputStream.ImageToBase64(500, 0, PixelFormat.Format32bppPArgb);
                if (imageBase64.NotIsNullOrEmpty())
                {
                    return Json(new { image = imageBase64, name = imageFile.FileName, isValid = isValid });    
                }
            }
            return Json(new { image = "", name = "NoFile", isValid = true });
        }

       

      
    }
}