using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AMPSchedules.ScheduledTasks;
using AMPSystem.Classes.Filters;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;

namespace AMPSystem.Classes
{
    public class AMPSystemFacade
    {
        // Object that will prevent simultaneous access to Repository bymultiple threads to prevent inconsistent data collect
        private static readonly object Lockobject = new object();

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
        private static ITimeTableItem GetItem(string itemName, DateTime startTime)
        {
            return ((List<ITimeTableItem>) TimeTableManager.Instance.TimeTable.ItemList).Find(
                i =>
                    (i.Name == itemName) &&
                    (i.StartTime == startTime));
        }

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
            var timeSpan = GetTimeSpan(units, time);
            var alertTime = item.StartTime - timeSpan;
            var alert = new Alert(alertTime, item);
            var dbAlert = AlertIntoBd(alertTime, item, Parameters(CurrentUser.Email, item));
            DbManager.Instance.SaveChanges();
            alert.Id = dbAlert.ID;
            item.Alerts.Add(alert);
            ScheduleAlert(item.Name, item.StartTime, alertTime, endTime, CurrentUser.Email, dbAlert.ID, item.Reminder);
        }

        /// <summary>
        ///     Returns the time stan
        /// </summary>
        /// <param name="units"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static TimeSpan GetTimeSpan(string units, int time)
        {
            switch (units)
            {
                case "Minutes":
                    return new TimeSpan(0, time, 0);
                case "Hours":
                    return new TimeSpan(time, 0, 0);
                case "Days":
                    return new TimeSpan(time, 0, 0, 0);
                case "Weeks":
                    return new TimeSpan(time*7, 0, 0, 0);
                default:
                    return new TimeSpan(0, 0, 0);
            }
        }

        /// <summary>
        /// Creates the BD dependences if not exists.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object[] Parameters(string email, ITimeTableItem item)
        {
            var room = item.Rooms.First();
            var mUser = DbManager.Instance.CreateUserIfNotExists(email);
            var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);

            var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
            var mRooms = (from iRoom in item.Rooms
                select DbManager.Instance.CreateRoomIfNotExists(mBuilding, iRoom.Floor, iRoom.Name)).ToList();
            if (item is EvaluationMoment) return new object[] {mUser, mRooms};
            return new object[] {mUser, mRoom};
        }

        /// <summary>
        /// Add alert into database.
        /// </summary>
        /// <param name="alertTime"></param>
        /// <param name="item"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static Models.Alert AlertIntoBd(DateTime alertTime, ITimeTableItem item, params object[] obj)
        {
            if (item is Lesson)
            {
                var mLesson = DbManager.Instance.CreateLessonIfNotExists(item.Name, (Models.Room) obj[1],
                    (Models.User) obj[0], item.Color, item.StartTime, item.EndTime);
                return DbManager.Instance.AddAlertToLesson(alertTime, mLesson);
            }
            if (item is EvaluationMoment)
            {
                var mEvMoment = DbManager.Instance.CreateEvaluationMomentIfNotExists(item.Name,
                    (List<Models.Room>) obj[1], (Models.User) obj[0], item.Color,
                    item.StartTime, item.EndTime, item.Description, null, item.Reminder);
                return DbManager.Instance.AddAlertToEvaluation(alertTime, mEvMoment);
            }
            //Office Hour
            var mOfficeHour = DbManager.Instance.CreateOfficeHourIfNotExists(item.Name, (Models.Room) obj[1],
                (Models.User) obj[0], item.Color, item.StartTime, item.EndTime);
            return DbManager.Instance.AddAlertToOfficeH(alertTime, mOfficeHour);
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
                a =>
                        a.Id == alertId);

            // Removes alert from the DB
            var dbAlert = DbManager.Instance.ReturnAlert(alertId);
            DbManager.Instance.RemoveAlert(dbAlert);

            item.Alerts.Remove(alert);
            return item.Alerts.OrderBy(x => x.AlertTime).ToList();
        }
        
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
            TimeTableItemBd(item);
        }

        /// <summary>
        /// Add lesson with reminder into bd.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="obj"></param>
        private static void LessonIntoDb(ITimeTableItem item, params object[] obj)
        {
            var mLesson = DbManager.Instance.ReturnLessonIfExists(item.Name, item.StartTime, item.EndTime, (Models.User)obj[0]);
            if (mLesson != null)
                DbManager.Instance.SaveLessonReminderChange(mLesson, item.Reminder);
            else
                DbManager.Instance.CreateLesson(item.Name, (Models.Room)obj[1], (Models.User)obj[0], item.Color, item.StartTime, item.EndTime);
            DbManager.Instance.SaveChanges();
        }

        /// <summary>
        /// Add office hours with reminder into bd.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="obj"></param>
        private static void OfficeHoursIntoBd(ITimeTableItem item, params object[] obj)
        {
            var mOfficeHours = DbManager.Instance.ReturnOfficeHourIfExists(item.Name, item.StartTime,
                item.EndTime, (Models.User)obj[0]);
            if (mOfficeHours != null)
                DbManager.Instance.SaveOfficeHourReminderChange(mOfficeHours, item.Reminder);
            else
                DbManager.Instance.CreateOfficeHour(item.Name, (Models.Room)obj[1], (Models.User)obj[0], item.Color, item.StartTime,
                    item.EndTime);
            DbManager.Instance.SaveChanges();
        }

        /// <summary>
        /// Add evaluation moments with reminder into bd.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="obj"></param>
        private static void EvaluationIntoDb(ITimeTableItem item, params object[] obj)
        {
            var mEvMoment = DbManager.Instance.CreateEvaluationMomentIfNotExists(item.Name,
                (List<Models.Room>)obj[1], (Models.User)obj[0], item.Color,
                item.StartTime, item.EndTime, item.Description, null, item.Reminder);
            if (mEvMoment != null)
                DbManager.Instance.SaveEvaluationReminderChange(mEvMoment, item.Reminder);
            else
                DbManager.Instance.CreateEvaluationMoment(item.Name, (List<Models.Room>)obj[1], (Models.User)obj[0], item.Color,
                        item.StartTime,
                        item.EndTime, item.Description, null, item.Reminder);
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
                switch (filter.Key)
                {
                    case "ClassName":
                        IFilter nameFilter = new Name(filter.Value);
                        mFilters.Add(nameFilter);
                        break;
                    case "Type":
                        IFilter typeFilter = new TypeF(filter.Value);
                        mFilters.Add(typeFilter);
                        break;
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
                    if (color == null) break;
                    item.Color = color;
                    TimeTableItemBd(item);
                }
            return ParseData();
        }

        /// <summary>
        /// Timetable item to database.
        /// </summary>
        /// <param name="item"></param>
        private void TimeTableItemBd(ITimeTableItem item)
        {
            if (item is Lesson)
            {
                LessonIntoDb(item, Parameters(CurrentUser.Email, item));
            }
            else if (item is OfficeHours)
            {
                OfficeHoursIntoBd(item, Parameters(CurrentUser.Email, item));
            }
            else if (item is EvaluationMoment)
            {
                EvaluationIntoDb(item, Parameters(CurrentUser.Email, item));
            }
        }
        
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
            var rooms = new List<Room>();
            var mRooms = new List<Models.Room>();
            UpdateRoomList(rooms, mRooms, buildingName, roomName);
            //Get The course
            ICollection<Course> courses = TimeTableManager.Instance.Repository.Courses.Where(mCourse => mCourse.Name == course).ToList();
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
        /// Creates the Buildings if not exist and update the list of rooms.
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="mRooms"></param>
        /// <param name="buildingName"></param>
        /// <param name="roomName"></param>
        private static void UpdateRoomList(ICollection<Room> rooms, ICollection<Models.Room> mRooms, string buildingName, string roomName)
        {
            foreach (var building in TimeTableManager.Instance.Repository.Buildings)
            {
                if (building.Name != buildingName) continue;
                foreach (var room in building.Rooms)
                {
                    if (room.Name != roomName) continue;
                    rooms.Add(room);
                    var mBulding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                    mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBulding, room.Floor, room.Name));
                }
            }
            if ((rooms.Count == 0) || (mRooms.Count == 0))
                throw new Exception("There is a problem with the selected room.");
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
            return TimeTableManager.Instance.TimeTable.ItemList.OfType<OfficeHours>().Cast<ITimeTableItem>().ToList();
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
            lock (Lockobject)
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
            roles.Add(domain == "student.uma.pt" ? "Student" : "Teacher");

            CurrentUser = Factory.Instance.CreateUser(user, mail.Address, roles, Repository.Instance.UserCourses);
            TimeTableManager.Instance.CreateTimeTable(start, end); //The manager will start the timetableitem list with the data read from the repo
        }

        /// <summary>
        ///     Parses TimeTableItems and converts it to CalendarItems
        /// </summary>
        /// <returns></returns>
        private static IList<CalendarItem> ParseData()
        {
            //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
            //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
            return TimeTableManager.Instance.TimeTable.ItemList.Select(item => new ItemAdapter(item)).Cast<CalendarItem>().ToList();
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
        /// <param name="reminder"></param>
        private static void ScheduleAlert(string name, DateTime startTime, DateTime alertTime, DateTime endTime, string email,
            int id, string reminder)
        {
            var job = JobBuilder.Create<EmailJob>()
                .UsingJobData("Name", name)
                .UsingJobData("StartTime", startTime.ToString("dd-MM-yyyy HH:mm"))
                .UsingJobData("EndTime", endTime.ToString("dd-MM-yyyy HH:mm"))
                .UsingJobData("Email", email)
                .UsingJobData("Reminder", reminder)
                .UsingJobData("Id", id)
                .Build();

            var trigger = TriggerBuilder.Create().StartAt(alertTime).Build();
            StdSchedulerFactory.GetDefaultScheduler().ScheduleJob(job, trigger);
        }
    }
}