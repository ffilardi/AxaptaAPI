using System.Web.Http;
using AxaptaApiApp.Handlers;

namespace AxaptaApiApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API handlers
            config.MessageHandlers.Add(new HttpsHandler());
            config.MessageHandlers.Add(new BasicAuthHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
