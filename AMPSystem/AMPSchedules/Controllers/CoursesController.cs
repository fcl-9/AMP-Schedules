using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    public class CoursesController : BaseController
    {
        // GET: Alerts
        public ActionResult Index()
        {
            var facade = PrepareAndGetFacade();

            return Content(JsonConvert.SerializeObject(facade.GetCourses(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}