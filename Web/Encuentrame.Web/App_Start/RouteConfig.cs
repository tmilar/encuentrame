using System.Web.Mvc;
using System.Web.Routing;

namespace Encuentrame.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "ManageOperationQuickCreate",
                url: "ManageOperation/QuickCreate/{partId}",
                defaults: new { controller = "ManageOperation", action = "QuickCreate", partId = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "ManagePartQuickAddComponent",
               url: "ManagePart/QuickAddComponent/{operationId}",
               defaults: new { controller = "ManagePart", action = "QuickAddComponent", operationId = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
