using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Newtonsoft.Json;
using Room = AMPSystem.Models.Room;

namespace AMPSchedules.Controllers
{
    public class ReminderController : Controller
    {
        public ActionResult Index()
        {
            var mail = ClaimsPrincipal.Current.FindFirst("preferred_username")?.Value;
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            var facade = new AMPSystemFacade(mail, startDateTime, endDateTime);

            return Content(JsonConvert.SerializeObject(facade.GetReminder(Request.QueryString["name"], Convert.ToDateTime(Request.QueryString["startTime"])),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }

        public void Add()
        {
            var mail = ClaimsPrincipal.Current.FindFirst("preferred_username")?.Value;
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            var facade = new AMPSystemFacade(mail, startDateTime, endDateTime);
            facade.AddReminder(Request.QueryString["name"], Convert.ToDateTime(Request.QueryString["startTime"]), Request.QueryString["reminder"]);
        }
    }
}