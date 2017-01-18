using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    public class BuildingsController : BaseController
    {
        // GET: Rooms
        public ActionResult Index()
        {
            var facade = PrepareAndGetFacade();

            return Content(JsonConvert.SerializeObject(facade.GetBuildings(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}