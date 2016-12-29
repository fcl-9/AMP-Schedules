using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Ajax.Utilities;
using Microsoft.Graph;
using Resources;

namespace AMPSchedules.Controllers
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

        public override ActionResult hook(TimeTableManager manager)
        {
            var item = ((List<ITimeTableItem>) manager.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["start"]) &&
                    i.EndTime == Convert.ToDateTime(Request.QueryString["end"]));
            // Remove event
            manager.TimeTable.RemoveTimeTableItem(item);
            return base.hook(manager);
        }
    }
}