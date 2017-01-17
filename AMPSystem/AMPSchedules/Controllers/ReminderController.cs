using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;

namespace AMPSchedules.Controllers
{
    public class ReminderController : Controller
    {
        public ActionResult Index()
        {
            var item = ((List<ITimeTableItem>)TimeTableManager.Instance.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));
            return Content(item.Reminder);
        }
        public void Add()
        {
            var reminder = Request.QueryString["reminder"];
        }

        public void Remove()
        {
            
        }

        public void Update()
        {
            
        }
    }
}