using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading;
using AMPSystem.Classes;
using AMPSystem.Classes.Filters;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Room = AMPSystem.Models.Room;

namespace AMPSystem
{
    public class AMPSystemFacade
    {
        private static readonly object _lockobject = new object();
        public User CurrentUser { get; private set; }

        public AMPSystemFacade(string email, DateTime start, DateTime end)
        {
            LoadData(email, start, end);
        }

        public ICollection<Course> GetCourses()
        {
            return CurrentUser.Courses;
        }

        public ICollection<Building> GetBuildings()
        {
            return TimeTableManager.Instance.Repository.Buildings;
        }

        public ICollection<Alert> GetAlerts()
        {
            return new List<Alert>();
        }

        public void AddAlert()
        {

        }

        public void RemoveAlert()
        {

        }

        public IDictionary<bool,string> GetReminder()
        {
            return new Dictionary<bool, string>();
        }

        public void AddReminder()
        {
            
        }

        public void RemoveRemind()
        {

        }

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

        public ICollection<CalendarItem> GetItems()
        {
            return ParseData();
        }

        public ICollection<CalendarItem> ChangeColor(string name, string color)
        {
            //Change the color on the items 
            foreach (var item in TimeTableManager.Instance.Repository.Items)
                if (item.Name == name)
                {
                    if (color != null)
                    {
                        item.Color = color;
                    }
                    else
                    {
                        break;
                    }
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

        public void AddEvent()
        {

        }

        public void RemoveEvent()
        {

        }
        public ICollection<ITimeTableItem> GetContacts()
        {
            return new List<ITimeTableItem>();
        }

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
            {
                roles.Add("Student");
            }
            else
            {
                roles.Add("Teacher");
            }

            CurrentUser = Factory.Instance.CreateUser(user, mail.Address, roles, Repository.Instance.UserCourses);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager.Instance.CreateTimeTable(start, end);
        }

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
    }
}