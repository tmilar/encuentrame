using System.Web.Mvc;

namespace Encuentrame.Web.Helpers
{
    public static class ModalReturnHelper
    {
        public static JsonResult ShowMessage( string modalMessage,string modalTitle)
        {
            return Result(true, modalMessage, modalTitle);
        }

        public static JsonResult ShowMessage( string modalMessage)
        {
            return Result(true, modalMessage, "");
        }
        public static JsonResult DontShowMessage( )
        {
            return Result(false, "", "");
        }

        private static JsonResult Result(bool showMessage, string modalMessage, string modalTitle)
        {
            var data = JsReturnHelper.Return(new { ShowMessage = showMessage, ModalTitle = modalTitle, ModalMessage = modalMessage });
            return new JsonResult() { Data = data };
        }

        public static JsonResult Error(string errorMessage="")
        {
            var data = JsReturnHelper.ReturnError(errorMessage);
            return new JsonResult() { Data = data };
        }
    }
}