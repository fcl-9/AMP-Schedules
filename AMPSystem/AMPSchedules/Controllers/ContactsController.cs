using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class ContactsController : Controller
    {
        // GET: ContactsController
        public ActionResult Index()
        {
            return View("Contacts");
        }

        public ActionResult Teachers()
        {
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses();
            loadData.GetRooms();
            loadData.GetTeachers();

            TimeTableManager manager = new TimeTableManager(loadData);
            foreach (var teacher in manager.Repository.Teachers)
            {
                Debug.Write(teacher);
            }
            return Content(JsonConvert.SerializeObject(manager.Repository.Teachers.ToArray()), "application/json");
        }
    }
}