using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.Filters;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Resources;
using Newtonsoft.Json;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
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
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error",
                    new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            return Content(JsonConvert.SerializeObject(manager.TimeTable.ItemList.ToArray(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");

        }
    }
}