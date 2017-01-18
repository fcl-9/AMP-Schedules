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
    public class ItemsController : BaseController
    {
        // GET: Items
        public ActionResult Index()
        {
            var facade = PrepareAndGetFacade();

            return Content(JsonConvert.SerializeObject(facade.GetItems(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }

        public ActionResult OfficeHours()
        {
            var facade = PrepareAndGetFacade();

            return Content(JsonConvert.SerializeObject(facade.GetContacts(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}