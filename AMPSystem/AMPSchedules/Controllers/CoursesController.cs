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
using User = Microsoft.Graph.User;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class CoursesController : TemplateController
    {
        GraphService graphService = new GraphService();
        // GET: Courses
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public override void hook(TimeTableManager manager)
        {
            var x = User;
        }
    }
}