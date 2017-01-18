using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;

namespace AMPSchedules.Controllers
{
    public abstract class BaseController : Controller
    {
        public AMPSystemFacade PrepareAndGetFacade()
        {
            var mail = ClaimsPrincipal.Current.FindFirst("preferred_username")?.Value;
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            return new AMPSystemFacade(mail, startDateTime, endDateTime);
        }
    }
}