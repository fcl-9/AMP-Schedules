using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMPSchedules.Controllers
{
    public class RoomsController : Controller
    {
        // GET: Rooms
        public ActionResult Index()
        {
            return View();
        }
    }
}