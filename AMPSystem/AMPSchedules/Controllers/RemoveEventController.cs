using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class RemoveEventController : TemplateController
    {
        // GET: AddEvent
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
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }

        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            var item = ((List<ITimeTableItem>) manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startEvent"]) &&
                    i.EndTime == Convert.ToDateTime(Request.QueryString["endEvent"]));
            // Remove event
            var dbItem = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime, item.EndTime);
            DbManager.Instance.RemoveEvent(dbItem);
            manager.RemoveTimeTableItem(item);
            return base.Hook(manager);
        }
    }
}