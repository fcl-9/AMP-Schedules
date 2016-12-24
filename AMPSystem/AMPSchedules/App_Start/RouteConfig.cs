using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace AMPSchedules
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

          //THIS CAN BE USED TO CREATE NEW ROUTES.
          //  routes.MapRoute(
          //    name: "Default",
          //    url: "{controller}/{action}/",
          //    defaults: new { controller = "", action = "" }
          //);
        }
    }
}
