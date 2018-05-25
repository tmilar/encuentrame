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
                "EventPersonMonitor",
                "EventMonitor/PersonMonitor/{eventId}/{userId}",
                new { controller = "EventMonitor", action = "PersonMonitor", eventId = 0 , userId = 0 }
            );

            routes.MapRoute(
                "EventMonitorGetItems",
                "EventMonitor/GetItems/{eventId}",
                new { controller = "EventMonitor", action = "GetItems", eventId =0}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
