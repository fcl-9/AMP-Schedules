using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class RemoveAlertController : TemplateController
    {
        // GET: RemoveAlert
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
            foreach (var key in Request.QueryString)
            {
                Debug.WriteLine(Request.QueryString[(string)key]);
            }

            var item = ((List<ITimeTableItem>)manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));

            var alert = ((List<Alert>)item.Alerts).Find(
                i =>
                    i.AlertID == int.Parse(Request.QueryString["id"]));

            //TODO Remove from DB
            item.Alerts.Remove(alert);
            var alerts = item.Alerts.OrderBy(x => x.Time).ToList();

            return Content(JsonConvert.SerializeObject(alerts.ToArray(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }
    }
}