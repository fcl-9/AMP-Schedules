using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Newtonsoft.Json;
using Resources;

namespace AMPSchedules.Controllers
{
    public class AlertController : TemplateController
    {
        // GET: Alert
        // GET: AddAlert
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            foreach (var key in Request.QueryString)
                Debug.WriteLine(Request.QueryString[(string) key]);

            var item = ((List<ITimeTableItem>) manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));

            var alerts = item.Alerts.OrderBy(x => x.AlertTime).ToList();
            Debug.WriteLine(alerts.Count);

            IDictionary<int, DateTime> data = new Dictionary<int, DateTime>();
            foreach (var t in alerts)
                data[t.Id] = t.AlertTime;

            //Order the alerts by time
            data.OrderBy(x => x.Value);
            return
                Content(
                    JsonConvert.SerializeObject(data.ToArray(),
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    "application/json");
        }
    }
}