using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using Microsoft.Graph;
using Newtonsoft.Json;
using Resources;
using AMPSystem.Interfaces;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
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
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            Debug.WriteLine("Message: " + Request.QueryString);

            foreach (var key in Request.QueryString)
            {
                Debug.WriteLine(Request.QueryString[(string) key]);
            }

            var item = ((List<ITimeTableItem>)manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]) &&
                    i.EndTime == Convert.ToDateTime(Request.QueryString["endTime"]));

            var alerts = item.Alerts.ToList();
            return Content(JsonConvert.SerializeObject(alerts.ToArray(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }
    }
}