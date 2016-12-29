﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
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
            loadData.GetUserCourses("2054313");
            loadData.GetSchedule("2054313");

            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager manager = new TimeTableManager(loadData, startDateTime, endDateTime);

            ICollection<ITimeTableItem> items = new List<ITimeTableItem>();

            foreach (var officeHour in manager.TimeTable.ItemList)
            {
                Debug.Write(officeHour);
                //if (officeHour == typeof(OfficeHours))
                //{
                    //officeHour.Teacher
                    //officeHour.Name;
                    //officeHour.Rooms;
                    items.Add(officeHour);
                //}
            }
            return Content(JsonConvert.SerializeObject(items.ToArray()), "application/json");
        }
    }
}