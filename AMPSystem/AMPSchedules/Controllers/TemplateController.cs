using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace AMPSystem.Classes
{
    abstract public class TemplateController: Controller
    {
        public abstract void hook(TimeTableManager manager);

        public async Task<ActionResult> TemplateMethod()
        {
            TimeTableManager manager = LoadData();
            hook(manager);
            IList<CalendarItem> parsedItems = ParseData(manager);
            return Content(JsonConvert.SerializeObject(parsedItems.ToArray()), "application/json");
        }

        public TimeTableManager LoadData()
        {
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses(Server.MapPath(@"~/App_Data/Cadeiras"));
            loadData.GetRooms(Server.MapPath(@"~/App_Data/Salas"));
            //!!!!!!!!!!!!!!!!!!!!!!! Commented only for tests!!!!!!!!!!!!!!!!!!!!!!!!
            //loadData.GetUserCourses(Server.MapPath(@"~/App_Data/Course/" + user));
            //loadData.GetSchedule(Server.MapPath(@"~/App_Data/Schedule/" + user));
            loadData.GetUserCourses(Server.MapPath(@"~/App_Data/Course/2054313"));
            loadData.GetSchedule(Server.MapPath(@"~/App_Data/Schedule/2054313"));
            loadData.GetTeachers(Server.MapPath(@"~/App_Data/Teacher"));
            //acaba
            //Default interval of the view
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            Timetable timetable = new Timetable(firstDayOfMonth, lastDayOfMonth);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager Manager = new TimeTableManager(timetable, loadData);
            return Manager;
        }

        

        public IList<CalendarItem> ParseData(TimeTableManager Manager)
        {
            IList<CalendarItem> parsedItems = new List<CalendarItem>();

            foreach (var item in Manager.TimeTable.ItemList)
            {
                CalendarItem adapter = new ItemAdapter(item);
                parsedItems.Add(adapter);
            }
            //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
            //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
            return parsedItems;
        }

    



    }
}
