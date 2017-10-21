using System.Net.Http.Headers;
using System.Web.Http;


namespace Encuentrame.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            config.MapHttpAttributeRoutes();
            //config.EnableCors(new EnableCorsAttribute("http://localhost,http://encuentrameweb.azurewebsites.net", "*","*"));

            config.Routes.MapHttpRoute(
                name: "AccountsApi",
                routeTemplate: "api/accounts",
                defaults: new { controller = "Account", action = "GetAll" }
            );
            config.Routes.MapHttpRoute(
                name: "ContactsApi",
                routeTemplate: "api/contacts",
                defaults: new { controller = "Contact", action = "GetAll" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
