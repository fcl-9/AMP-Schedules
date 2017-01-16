using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using Newtonsoft.Json;
using Resources;

namespace AMPSchedules.Controllers
{
    public class CoursesController : TemplateController
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

        public override ActionResult Hook()
        {
            return Content(JsonConvert.SerializeObject(CurrentUser.Courses), "application/json");
        }
    }
}