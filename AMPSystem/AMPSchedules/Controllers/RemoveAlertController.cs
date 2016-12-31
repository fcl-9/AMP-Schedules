using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.DAL;
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
                //Debug.WriteLine("Message:" + Request.QueryString[(string)key]);
            }

            var item = ((List<ITimeTableItem>)manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));
            ////Mockobject
            //var alerts = new List<Alert>
            //{
            //    new Alert(new TimeSpan(13, 00, 59), item),
            //    new Alert(new TimeSpan(14, 30, 00), item)
            //};
            //alerts[0].AlertID = 0;
            //alerts[1].AlertID = 1;
            //item.Alerts = alerts;
            ////Mockobject

            var alert = ((List<Alert>)item.Alerts).Find(
                i =>
                    i.Id == int.Parse(Request.QueryString["alertId"]));

            // Removes alert from the DB
            var dbAlert = DbManager.Instance.ReturnAlert(int.Parse(Request.QueryString["alertId"]));
            DbManager.Instance.RemoveAlert(dbAlert);

            item.Alerts.Remove(alert);
            IDictionary<int, DateTime> data = new Dictionary<int, DateTime>();
            foreach (var t in item.Alerts)
            {
                data[t.Id] = t.AlertTime;
            }

            //Order the alerts by time
            data.OrderBy(x => x.Value);
            return Content(JsonConvert.SerializeObject(data.ToArray(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }
    }
}