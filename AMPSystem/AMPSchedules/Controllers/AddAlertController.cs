using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class AddAlertController : TemplateController
    {
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
            var keys = Request.QueryString.AllKeys;
            for (var i = 0 ; i < keys.Length - 2; i = i + 5)
            {
                var name = Request.QueryString[i];
                var startTime = Convert.ToDateTime(Request.QueryString[i + 1]);
                var endTime = Convert.ToDateTime(Request.QueryString[i + 2]);

                var time = int.Parse(Request.QueryString[i + 3]);
                var units = Request.QueryString[i + 4];
                
                Debug.WriteLine(name + " " + startTime + " " + endTime + " " + time + " " + units);

                var item = ((List<ITimeTableItem>) manager.TimeTable.ItemList).Find(
                    it =>
                        it.Name == name &&
                        it.StartTime == startTime &&
                        it.EndTime == endTime);

                TimeSpan timeSpan;
                switch (units)
                {
                    case "Minutes":
                        timeSpan = new TimeSpan(0, time, 0);
                        break;
                    case "Hours":
                        timeSpan = new TimeSpan(time, 0, 0);
                        break;
                    case "Days":
                        timeSpan = new TimeSpan(time, 0, 0, 0);
                        break;
                    case "Weeks":
                        timeSpan = new TimeSpan(time * 7, 0, 0, 0);
                        break;
                    default:
                        timeSpan = new TimeSpan(0, 0, 0);
                        break;
                }
                var alert = new Alert(timeSpan, item);
                item.Alerts.Add(alert);
                //TODO Add into BD
            }
            return base.Hook(manager);
        }
    }
}