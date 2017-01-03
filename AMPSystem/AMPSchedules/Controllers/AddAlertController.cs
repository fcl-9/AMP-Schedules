using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSchedules.ScheduledTasks;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Quartz;
using Quartz.Impl;
using Resources;
using Room = AMPSystem.Models.Room;

namespace AMPSchedules.Controllers
{
    public class AddAlertController : TemplateController
    {
        // GET: AddAlert
        [HttpGet]
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
            var keys = Request.QueryString.AllKeys;
            for (var i = 0; i < keys.Length - 2; i = i + 5)
            {
                var name = Request.QueryString[i];
                var startTime = Convert.ToDateTime(Request.QueryString[i + 1]);
                var endTime = Convert.ToDateTime(Request.QueryString[i + 2]);

                var time = int.Parse(Request.QueryString[i + 3]);
                var units = Request.QueryString[i + 4];

                Debug.WriteLine(name + " " + startTime + " " + endTime + " " + time + " " + units);

                var item = ((List<ITimeTableItem>) TimeTableManager.Instance.TimeTable.ItemList).Find(
                    it =>
                        it.Name == name &&
                        it.StartTime == startTime);

                TimeSpan timeSpan;
                switch (units)
                {
                    case "Minutes":
                        timeSpan = new TimeSpan(0, time, 0);
                        break;
                    case "Hours":
                        timeSpan = new TimeSpan(time, 0, 0);
                        break;
                    case "Days":
                        timeSpan = new TimeSpan(time, 0, 0, 0);
                        break;
                    case "Weeks":
                        timeSpan = new TimeSpan(time * 7, 0, 0, 0);
                        break;
                    default:
                        timeSpan = new TimeSpan(0, 0, 0);
                        break;
                }
                var alertTime = item.StartTime - timeSpan;
                var alert = new Alert(alertTime, item);
                AMPSystem.Models.Alert dbAlert = null;
                // Add alert to the DB
                if (item is Lesson)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(item.Rooms.First().Building.Name);
                    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, item.Rooms.First().Floor,
                        item.Rooms.First().Name);
                    var mLesson = DbManager.Instance.CreateLessonIfNotExists(item.Name, mRoom, mUser, item.Color,
                        item.StartTime, item.EndTime);
                    dbAlert = DbManager.Instance.AddAlertToLesson(alertTime, mLesson);
                    DbManager.Instance.SaveChanges();
                }
                else if (item is EvaluationMoment)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mRooms = new List<Room>();
                    foreach (var room in item.Rooms)
                    {
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        mRooms.Add(mRoom);
                    }
                    var mEvMoment = DbManager.Instance.CreateEvaluationMomentIfNotExists(item.Name, mRooms, mUser,
                            item.Color,
                            item.StartTime, item.EndTime, item.Description, null);
                        // Courses could be null since this is an event that came from the API
                    dbAlert = DbManager.Instance.AddAlertToEvaluation(alertTime, mEvMoment);
                    DbManager.Instance.SaveChanges();
                }
                else if (item is OfficeHours)
                {
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(item.Rooms.First().Building.Name);
                    var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, item.Rooms.First().Floor,
                        item.Rooms.First().Name);
                    var mOfficeHour = DbManager.Instance.CreateOfficeHourIfNotExists(item.Name, mRoom, mUser, item.Color,
                        item.StartTime, item.EndTime);
                    dbAlert = DbManager.Instance.AddAlertToOfficeH(alertTime, mOfficeHour);
                    DbManager.Instance.SaveChanges();
                }
                item.Alerts.Add(alert);
                ScheduleAlert(item.Name, item.StartTime, alertTime, endTime, CurrentUser.Email, dbAlert.ID);
            }
            return base.Hook();
        }

        /// <summary>
        ///     Schedules an email for the alert
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="alertTime"></param>
        /// <param name="endTime"></param>
        /// <param name="email"></param>
        /// <param name="id"></param>
        private void ScheduleAlert(string name, DateTime startTime, DateTime alertTime, DateTime endTime, string email,
            int id)
        {
            var job = JobBuilder.Create<EmailJob>()
                .UsingJobData("Name", name)
                .UsingJobData("StartTime", startTime.ToString("dd-MM-yyyy HH:mm"))
                .UsingJobData("EndTime", endTime.ToString("dd-MM-yyyy HH:mm"))
                .UsingJobData("Email", email)
                .UsingJobData("Id", id)
                .Build();

            var trigger = TriggerBuilder.Create().StartAt(alertTime).Build();

            StdSchedulerFactory.GetDefaultScheduler().ScheduleJob(job, trigger);
        }
    }
}