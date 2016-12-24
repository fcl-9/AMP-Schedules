using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using AMPSystem.Classes;
using AMPSystem.Interfaces;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class DataLoaderController : Controller
    {
        // GET: DataLoader
        public ActionResult ShowLoadData()
        {
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses(Server.MapPath(@"~/App_Data/Cadeiras"));
            ViewBag.Courses = loadData.Courses;
            return View();
        }
        
    }
}