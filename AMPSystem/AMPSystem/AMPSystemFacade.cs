using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AMPSchedules.ScheduledTasks;
using AMPSystem.Classes;
using AMPSystem.Classes.Filters;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using Room = AMPSystem.Models.Room;

namespace AMPSystem
{
    public class AMPSystemFacade
    {
        // Object that will prevent simultaneous access to Repository bymultiple threads to prevent inconsistent data collect
        private static readonly object _lockobject = new object();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="email">Email of the current user</param>
        /// <param name="start">Date of the begining of the view range</param>
        /// <param name="end">Date of the end of the view range</param>
        public AMPSystemFacade(string email, DateTime start, DateTime end)
        {
            LoadData(email, start, end);
        }

        // Saves the current user
        public User CurrentUser { get; private set; }

        /// <summary>
        ///     Returns all the courses from the current user
        /// </summary>
        /// <returns></returns>
        public ICollection<Course> GetCourses()
        {
            return CurrentUser.Courses;
        }

        /// <summary>
        ///     Returns all the buldings
        /// </summary>
        /// <returns></returns>
        public ICollection<Building> GetBuildings()
        {
            return TimeTableManager.Instance.Repository.Buildings;
        }

        /// <summary>
        ///     Return all alerts of a given Item
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="startTime">Start time of the item</param>
        /// <returns></returns>
        public ICollection<Alert> GetAlerts(string itemName, DateTime startTime)
        {
            var item = GetItem(itemName, startTime);

            return item.Alerts.OrderBy(x => x.AlertTime).ToList();
        }

        /// <summary>
        ///     Returns the item with a given name and start time
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="startTime">Start time of the item</param>
        /// <returns></returns>
        private ITimeTableItem GetItem(string itemName, DateTime startTime)
        {
            return ((List<ITimeTableItem>)TimeTableManager.Instance.TimeTable.ItemList).Find(
                i =>
                    i.Name == itemName &&
                    i.StartTime == startTime);
        }

        //TODO: Needs refactor
        /// <summary>
        ///     Add an alert to an Item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="startTime">Start time of the item</param>
        /// <param name="endTime">End time of the item</param>
        /// <param name="time">Time range</param>
        /// <param name="units">Time range unit</param>
        public void AddAlert(string name, DateTime startTime, DateTime endTime, int time, string units)
        {
            var item = GetItem(name, startTime);

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
            if (alertTime <= DateTime.Now)
            {
                throw new Exception("You're trying to schedule an alert for a date previous than current date.");
            }
            var alert = new Alert(alertTime, item);
            Models.Alert dbAlert = null;
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
                    item.StartTime, item.EndTime, item.Description, null, item.Reminder);
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
            alert.Id = dbAlert.ID;
            item.Alerts.Add(alert);
            ScheduleAlert(item.Name, item.StartTime, alertTime, endTime, CurrentUser.Email, dbAlert.ID);
        }

        /// <summary>
        ///     Removes an alert from an item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="startTime">Start time of the item</param>
        /// <param name="alertId">The ID of the alert</param>
        /// <returns></returns>
        public ICollection<Alert> RemoveAlert(string name, DateTime startTime, int alertId)
        {
            var item = GetItem(name, startTime);

            var alert = ((List<Alert>) item.Alerts).Find(
                i =>
                    i.Id == alertId);

            // Removes alert from the DB
            var dbAlert = DbManager.Instance.ReturnAlert(alertId);
            DbManager.Instance.RemoveAlert(dbAlert);

            item.Alerts.Remove(alert);
            return item.Alerts.OrderBy(x => x.AlertTime).ToList();
        }

        /// TODO: Needs refactor
        /// <summary>
        ///     Adds a reminder to an item
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="itemStartTime">Start time of the item</param>
        /// <param name="reminder">Reminder to add</param>
        public void AddReminder(string itemName, DateTime itemStartTime, string reminder)
        {
            var item = GetItem(itemName, itemStartTime);

            item.Reminder = reminder;
            var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
            if (item is Lesson)
            {
                var room = item.Rooms.First();
                var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                var mLesson = DbManager.Instance.ReturnLessonIfExists(item.Name, item.StartTime, item.EndTime, mUser);
                if (mLesson == null)
                    DbManager.Instance.CreateLesson(item.Name, mRoom, mUser, item.Color, item.StartTime,
                        item.EndTime);
                else
                    DbManager.Instance.SaveLessonReminderChange(mLesson, item.Reminder);
            }
            else if (item is OfficeHours)
            {
                var room = item.Rooms.First();
                var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                var mOfficeHours = DbManager.Instance.ReturnOfficeHourIfExists(item.Name, item.StartTime,
                    item.EndTime, mUser);
                if (mOfficeHours == null)
                    DbManager.Instance.CreateOfficeHour(item.Name, mRoom, mUser, item.Color, item.StartTime,
                        item.EndTime);
                else
                    DbManager.Instance.SaveOfficeHourReminderChange(mOfficeHours, item.Reminder);
            }
            else if (item is EvaluationMoment)
            {
                var mRooms = new List<Room>();
                foreach (var room in item.Rooms)
                {
                    var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                    mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name));
                }

                var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime,
                    item.EndTime, mUser);
                if (mEvaluation == null)
                    DbManager.Instance.CreateEvaluationMoment(item.Name, mRooms, mUser, item.Color,
                        item.StartTime,
                        item.EndTime, item.Description, null, item.Reminder);
                // Courses could be null since this is an event that came from the API
                else
                    DbManager.Instance.SaveEvaluationReminderChange(mEvaluation, item.Reminder);
            }
            DbManager.Instance.SaveChanges();
        }

        /// <summary>
        ///     Returns all the reminders of an item
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="itemStartTime">Start time of the item</param>
        /// <returns></returns>
        public JObject GetReminder(string itemName, DateTime itemStartTime)
        {
            var item = GetItem(itemName, itemStartTime);

            var result = new JObject();
            if (item.Reminder == null)
            {
                result["Success"] = false;
                result["Message"] = "There is no Reminder for this event";
            }
            else
            {
                result["Success"] = true;
                result["Message"] = item.Reminder;
            }
            return result;
        }

        /// <summary>
        ///     Adds a filter to the calendar
        /// </summary>
        /// <param name="filters">Dictionary formed by filter type and name</param>
        /// <returns></returns>
        public ICollection<CalendarItem> AddFilter(Dictionary<string, string> filters)
        {
            var mFilters = new AndCompositeFilter();
            foreach (var filter in filters)
                if (filter.Key == "ClassName")
                {
                    IFilter nameFilter = new Name(filter.Value);
                    mFilters.Add(nameFilter);
                }
                else if (filter.Key == "Type")
                {
                    IFilter typeFilter = new TypeF(filter.Value);
                    mFilters.Add(typeFilter);
                }
            mFilters.ApplyFilter();
            return ParseData();
        }

        /// <summary>
        ///     Returns all the items
        /// </summary>
        /// <returns></returns>
        public ICollection<CalendarItem> GetItems()
        {
            return ParseData();
        }

        //TODO: Need refactor
        /// <summary>
        ///     Changes the color of the items
        /// </summary>
        /// <param name="name">The name of the item we want to change</param>
        /// <param name="color">The color we want to put</param>
        /// <returns></returns>
        public ICollection<CalendarItem> ChangeColor(string name, string color)
        {
            //Change the color on the items 
            foreach (var item in TimeTableManager.Instance.Repository.Items)
                if (item.Name == name)
                {
                    if (color != null)
                        item.Color = color;
                    else
                        break;
                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    if (item is Lesson)
                    {
                        var room = item.Rooms.First();
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        var mLesson = DbManager.Instance.ReturnLessonIfExists(item.Name, item.StartTime, item.EndTime,
                            mUser);
                        if (mLesson == null)
                            DbManager.Instance.CreateLesson(item.Name, mRoom, mUser, item.Color, item.StartTime,
                                item.EndTime);
                        else
                            DbManager.Instance.SaveLessonColorChange(mLesson, item.Color);
                    }
                    else if (item is OfficeHours)
                    {
                        var room = item.Rooms.First();
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        var mOfficeHours = DbManager.Instance.ReturnOfficeHourIfExists(item.Name, item.StartTime,
                            item.EndTime, mUser);
                        if (mOfficeHours == null)
                            DbManager.Instance.CreateOfficeHour(item.Name, mRoom, mUser, item.Color, item.StartTime,
                                item.EndTime);
                        else
                            DbManager.Instance.SaveOfficeHourColorChange(mOfficeHours, item.Color);
                    }
                    else if (item is EvaluationMoment)
                    {
                        var mRooms = new List<Room>();
                        foreach (var room in item.Rooms)
                        {
                            var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                            mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name));
                        }

                        var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime,
                            item.EndTime, mUser);
                        if (mEvaluation == null)
                            DbManager.Instance.CreateEvaluationMoment(item.Name, mRooms, mUser, item.Color,
                                item.StartTime,
                                item.EndTime, item.Description, null, item.Reminder);
                        // Courses could be null since this is an event that came from the API
                        else
                            DbManager.Instance.SaveEvaluationColorChange(mEvaluation, item.Color);
                    }
                    DbManager.Instance.SaveChanges();
                }
            return ParseData();
        }

        //TODO: Need refactor
        /// <summary>
        ///     Adds a new custom event
        /// </summary>
        /// <param name="buildingName">The name of the building where the event will be</param>
        /// <param name="roomName">The name of the room where the event will be</param>
        /// <param name="course">The course name wich the event is related to</param>
        /// <param name="startTime">The start time of the event</param>
        /// <param name="endTime">The end time of the event</param>
        /// <param name="name">The name of the event</param>
        /// <param name="description">The description of the event</param>
        /// <param name="reminder">The reminder to the event</param>
        /// <returns></returns>
        public ICollection<CalendarItem> AddEvent(string buildingName, string roomName, string course,
            DateTime startTime, DateTime endTime, string name, string description, string reminder)
        {
            var rooms = new List<Classes.Room>();
            var mRooms = new List<Room>();
            foreach (var building in TimeTableManager.Instance.Repository.Buildings)
                if (building.Name == buildingName)
                    foreach (var room in building.Rooms)
                        if (room.Name == roomName)
                        {
                            rooms.Add(room);
                            var mBulding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                            mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBulding, room.Floor, room.Name));
                        }
            if (rooms.Count == 0 || mRooms.Count == 0)
                throw new Exception("There is a problem with the selected room.");

            //Get The course
            ICollection<Course> courses = new List<Course>();
            foreach (var mCourse in TimeTableManager.Instance.Repository.Courses)
                if (mCourse.Name == course)
                    courses.Add(mCourse);

            if (courses.Count == 0)
                throw new Exception("There is a problem with the selected course.");


            //Get the course
            ITimeTableItem newEvent = new EvaluationMoment(startTime, endTime, rooms, courses, name, description, true,
                reminder);
            //Add new Event
            Repository.Instance.Items.Add(newEvent);
            TimeTableManager.Instance.AddTimetableItem(newEvent);
            var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
            DbManager.Instance.CreateEvaluationMoment(name, mRooms, mUser, null, startTime, endTime, description,
                courses.First().Name, reminder);
            DbManager.Instance.SaveChanges();
            return ParseData();
        }

        /// <summary>
        ///     Removes a custom event
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="startTime">Start time of the item</param>
        /// <returns></returns>
        public ICollection<CalendarItem> RemoveEvent(string itemName, DateTime startTime)
        {
            var item = GetItem(itemName, startTime);

            // Remove event
            var dbUser = DbManager.Instance.ReturnUserIfExists(CurrentUser.Email);
            var dbItem = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime, item.EndTime,
                dbUser);
            DbManager.Instance.RemoveEvent(dbItem);
            TimeTableManager.Instance.RemoveTimeTableItem(item);
            Repository.Instance.Items.Remove(item);
            return ParseData();
        }

        /// <summary>
        ///     Returns all office hours
        /// </summary>
        /// <returns></returns>
        public ICollection<ITimeTableItem> GetOfficeHours()
        {
            ICollection<ITimeTableItem> items = new List<ITimeTableItem>();
            foreach (var officeHour in TimeTableManager.Instance.TimeTable.ItemList)
                if (officeHour is OfficeHours)
                    items.Add(officeHour);
            return items;
        }

        /// <summary>
        ///     Load the data from repository
        /// </summary>
        /// <param name="email">Email of the current user</param>
        /// <param name="start">Date of the begining of the view range</param>
        /// <param name="end">Date of the end of the view range</param>
        private void LoadData(string email, DateTime start, DateTime end)
        {
            var mail = new MailAddress(email);

            var user = mail.User;
            lock (_lockobject)
            {
                if (!Repository.Instance.DataLoaded)
                {
                    IDataReader dataReader = new FileData();
                    Repository.Instance.DataReader = dataReader;
                    Repository.Instance.CleanRepository();
                    Repository.Instance.LoadData(mail);
                }
            }

            var roles = new List<string>();

            var domain = mail.Host;
            if (domain == "student.uma.pt")
                roles.Add("Student");
            else
                roles.Add("Teacher");

            CurrentUser = Factory.Instance.CreateUser(user, mail.Address, roles, Repository.Instance.UserCourses);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager.Instance.CreateTimeTable(start, end);
        }

        /// <summary>
        ///     Parses TimeTableItems and converts it to CalendarItems
        /// </summary>
        /// <returns></returns>
        private IList<CalendarItem> ParseData()
        {
            IList<CalendarItem> parsedItems = new List<CalendarItem>();

            foreach (var item in TimeTableManager.Instance.TimeTable.ItemList)
            {
                CalendarItem adapter = new ItemAdapter(item);
                parsedItems.Add(adapter);
            }
            //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
            //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
            return parsedItems;
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