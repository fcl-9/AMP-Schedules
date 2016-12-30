using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Graph;
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
            var item = ((List<ITimeTableItem>)manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["start"]) &&
                    i.EndTime == Convert.ToDateTime(Request.QueryString["end"]));

            TimeSpan timeSpan;
            switch (Request.QueryString["timeUnit"])
            {
                case "Minutes":
                    timeSpan = new TimeSpan(0, int.Parse(Request.QueryString["number"]),0);
                    break;
                case "Hours":
                    timeSpan = new TimeSpan(int.Parse(Request.QueryString["number"]), 0, 0);
                    break;
                case "Days":
                    timeSpan = new TimeSpan(int.Parse(Request.QueryString["number"]), 0, 0, 0);
                    break;
                case "Weeks":
                    timeSpan = new TimeSpan(int.Parse(Request.QueryString["number"]) * 7, 0, 0, 0);
                    break;
                default:
                    timeSpan = new TimeSpan();
                    break;
            }
            var alert = new Alert(timeSpan, item);
            item.Alerts.Add(alert);
            return base.Hook(manager);
        }
    }
}