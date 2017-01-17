using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Resources;

namespace AMPSchedules.Controllers
{
    public class OldRemoveEventController : OldTemplateController
    {
        // GET: AddEvent
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

        public override ActionResult Hook()
        {
            var item = ((List<ITimeTableItem>)TimeTableManager.Instance.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startEvent"]) &&
                    i.EndTime == Convert.ToDateTime(Request.QueryString["endEvent"]));
            // Remove event
            var dbUser = DbManager.Instance.ReturnUserIfExists(CurrentUser.Email);
            var dbItem = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime, item.EndTime, dbUser);
            DbManager.Instance.RemoveEvent(dbItem);
            TimeTableManager.Instance.RemoveTimeTableItem(item);
            Repository.Instance.Items.Remove(item);
            return base.Hook();
        }
    }
}