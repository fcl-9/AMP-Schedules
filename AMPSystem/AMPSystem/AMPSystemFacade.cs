using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public void AddFilter()
        {
            
        }

        public ICollection<CalendarItem> GetItems()
        {
            return ParseData();
        }

        public void ChangeColor()
        {
            
        }

        public ICollection<CalendarItem> GetEvents()
        {
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