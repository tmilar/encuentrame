using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class HtmlRequestHelper
    {
        private static string version = "";

        static HtmlRequestHelper()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            version = assembly.GetName().Version.ToString();
        }
        public static string ControllerName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string ActionName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }

        public static string ScriptUrl(this HtmlHelper htmlHelper, string virtualPath)
        {
            var url = Scripts.Url(virtualPath);

            return "{0}?v={1}".FormatWith(url.ToHtmlString(), version);
        }
    }
}