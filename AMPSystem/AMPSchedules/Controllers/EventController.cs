using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using Newtonsoft.Json;
using Resources;

namespace AMPSchedules.Controllers
{
    public class EventController : TemplateController
    {
        // GET: Event
        public ActionResult Index()
        {
            return View($"Events");
        }

        public async Task<ActionResult> ReturnEvents()
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

        //public override ActionResult Hook()
        //{
        //    return
        //        Content(
        //            JsonConvert.SerializeObject(TimeTableManager.Instance.TimeTable.ItemList.ToArray(),
        //                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
        //            "application/json");
        //}
    }
}