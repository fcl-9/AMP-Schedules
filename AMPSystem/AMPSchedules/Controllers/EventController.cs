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
                return RedirectToAction($"Index", $"Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }

        //TODO DELETE
        public override ActionResult Hook()
        {
            //TODO: MOCKOBJ
            foreach (var item in TimeTableManager.Instance.TimeTable.ItemList)
            {
                var random = new Random();
                item.Reminder = "REMIMDER " + random.Next(0, 100);
            }

            return base.Hook();
        }
    }
}