using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMPSchedules.Controllers
{
    public class ColorController : BaseController
    {
        // GET: Color/Add
        public ActionResult Add()
        {
            var facade = PrepareAndGetFacade();

            //Read The Color that was sent
            string color = null;
            string itemName = null;
            foreach (var eventName in Request.QueryString)
                if ((string)eventName != "start" && (string)eventName != "end")
                {
                    itemName = (string)eventName;
                    color = Request.QueryString[itemName];
                }
            return Content(JsonConvert.SerializeObject(facade.ChangeColor(itemName,color),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}