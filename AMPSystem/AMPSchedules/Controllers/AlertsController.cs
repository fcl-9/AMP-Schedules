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
    public class AlertsController : BaseController
    {
        // GET: Courses
        public ActionResult Index()
        {
            var facade = PrepareAndGetFacade();

            IDictionary<int, DateTime> data = new Dictionary<int, DateTime>();
            foreach (var t in facade.GetAlerts(Request.QueryString["name"], Convert.ToDateTime(Request.QueryString["startTime"])))
                data[t.Id] = t.AlertTime;

            //Order the alerts by time
            data.OrderBy(x => x.Value);
            return
                Content(
                    JsonConvert.SerializeObject(data,
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }

        public ActionResult Add()
        {
            var facade = PrepareAndGetFacade();

            var keys = Request.QueryString.AllKeys;
            for (var i = 0; i < keys.Length - 2; i = i + 5)
            {
                var name = Request.QueryString[i];
                var startTime = Convert.ToDateTime(Request.QueryString[i + 1]);
                var endTime = Convert.ToDateTime(Request.QueryString[i + 2]);

                var time = int.Parse(Request.QueryString[i + 3]);
                var units = Request.QueryString[i + 4];

                try
                {
                    facade.AddAlert(name, startTime, endTime, time, units);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, responseText = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Content(JsonConvert.SerializeObject(facade.GetItems(),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }

        public ActionResult Remove()
        {
            var facade = PrepareAndGetFacade();

            IDictionary<int, DateTime> data = new Dictionary<int, DateTime>();
            foreach (var t in facade.RemoveAlert(Request.QueryString["name"], Convert.ToDateTime(Request.QueryString["startTime"]), int.Parse(Request.QueryString["alertId"])))
                data[t.Id] = t.AlertTime;

            //Order the alerts by time
            data.OrderBy(x => x.Value);
            return
                Content(
                    JsonConvert.SerializeObject(data,
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
        }
    }
}