using System.Net.Mail;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AMPSchedules.ScheduledTasks;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;

namespace AMPSchedules
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JobScheduler.Start();
            
        }
    }
}
