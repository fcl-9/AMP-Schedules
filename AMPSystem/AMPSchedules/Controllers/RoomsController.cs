using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using AMPSystem.Classes;
using Microsoft.Graph;
using Newtonsoft.Json;
using Resources;
using User = Microsoft.Graph.User;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class RoomsController : TemplateController
    {
        GraphService graphService = new GraphService();

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            try
            {

                return await TemplateMethod();

            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult hook(TimeTableManager manager)
        {
            return Content(JsonConvert.SerializeObject(manager.Repository.Buildings), "application/json");
            
        }
    }
}