using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMPSchedules.Controllers
{
    public class FiltersController : Controller
    {
        // GET: Filters/Add
        public ActionResult Add()
        {
            return View();
        }
    }
}