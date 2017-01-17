using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Room = AMPSystem.Models.Room;

namespace AMPSchedules.Controllers
{
    public class ReminderController : Controller
    {
        public ActionResult Index()
        {
            TimeTableManager.Instance.CreateTimeTable(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            var item = ((List<ITimeTableItem>)TimeTableManager.Instance.TimeTable.ItemList).Find(
                i =>
                    i.Name == Request.QueryString["name"] &&
                    i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));
            return Content(item.Reminder);
        }
        public void Add()
        {
            //TimeTableManager.Instance.CreateTimeTable(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            //var item = ((List<ITimeTableItem>)TimeTableManager.Instance.TimeTable.ItemList).Find(
            //    i =>
            //        i.Name == Request.QueryString["name"] &&
            //        i.StartTime == Convert.ToDateTime(Request.QueryString["startTime"]));
            //var reminder = Request.QueryString["reminder"];
            //var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
            //if (item is Lesson)
            //{
            //    var room = item.Rooms.First();
            //    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
            //    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
            //    var mLesson = DbManager.Instance.ReturnLessonIfExists(item.Name, item.StartTime, item.EndTime, mUser);
            //    if (mLesson == null)
            //        DbManager.Instance.CreateLesson(item.Name, mRoom, mUser, item.Color, item.StartTime,
            //            item.EndTime);
            //    else
            //        DbManager.Instance.SaveLessonReminderChange(mLesson, item.Reminder);
            //}
            //else if (item is OfficeHours)
            //{
            //    var room = item.Rooms.First();
            //    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
            //    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
            //    var mOfficeHours = DbManager.Instance.ReturnOfficeHourIfExists(item.Name, item.StartTime,
            //        item.EndTime, mUser);
            //    if (mOfficeHours == null)
            //        DbManager.Instance.CreateOfficeHour(item.Name, mRoom, mUser, item.Color, item.StartTime,
            //            item.EndTime);
            //    else
            //        DbManager.Instance.SaveOfficeHourReminderChange(mOfficeHours, item.Reminder);
            //}
            //else if (item is EvaluationMoment)
            //{
            //    var mRooms = new List<Room>();
            //    foreach (var room in item.Rooms)
            //    {
            //        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
            //        mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name));
            //    }

            //    var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime,
            //        item.EndTime, mUser);
            //    if (mEvaluation == null)
            //        DbManager.Instance.CreateEvaluationMoment(item.Name, mRooms, mUser, item.Color,
            //                item.StartTime,
            //                item.EndTime, item.Description, null, item.Reminder);
            //     Courses could be null since this is an event that came from the API
            //    else
            //        DbManager.Instance.SaveEvaluationReminderChange(mEvaluation, item.Reminder);
            //}
            //DbManager.Instance.SaveChanges();
        }

        public void Remove()
        {
            
        }

        public void Update()
        {
            
        }
    }
}