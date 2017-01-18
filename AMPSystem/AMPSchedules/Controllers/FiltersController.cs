using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    public class FiltersController : BaseController
    {
        // GET: Filters/Add
        public ActionResult Add()
        {
            var facade = PrepareAndGetFacade();

            var filters = new Dictionary<string, string>();
            foreach (var filter in Request.QueryString)
            {
                filters[Request.QueryString[(string) filter]] = (string) filter;
            }
            return Content(JsonConvert.SerializeObject(facade.AddFilter(filters),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}