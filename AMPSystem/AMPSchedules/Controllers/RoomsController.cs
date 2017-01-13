using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using Newtonsoft.Json;
using Resources;

namespace AMPSchedules.Controllers
{
    public class RoomsController : TemplateController
    {
        // GET: Courses
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

        public override ActionResult Hook(TimeTableManager manager)
        {
            return
                Content(
                    JsonConvert.SerializeObject(manager.Repository.Buildings,
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    "application/json");
        }
    }
}