namespace Encuentrame.Web.Helpers
{
    public static class JsReturnHelper
    {
        public static dynamic ReturnError( string errorMessage="")
        {
            return new {Status = false, ErrorMessage=errorMessage};
        }
        public static dynamic Return(dynamic info)
        {
            return new { Status = true, Info = info };
        }

        public static object ReturnOk()
        {
            return new {Status = true,};
        }
    }
}