using System;
using System.Collections.Generic;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMPSystem
{
    public class AMPSystemFacade
    {
        public Repository Repository { get; set; }
        public TimeTableManager TimeTableManager { get; set; }

        public ICollection<Course> GetCourses()
        {
            
        }

        public ICollection<Room> GetRooms()
        {
            
        }

        public ICollection<Alert> GetAlerts()
        {
            
        }

        public void AddAlert()
        {

        }

        public void RemoveAlert()
        {

        }

        public IDictionary<bool,string> GetReminder()
        {

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

        public ICollection<ITimeTableItem> GetItems()
        {
            
        }

        public void ChangeColor()
        {
            
        }

        public ICollection<ITimeTableItem> GetEvents()
        {

        }

        public void AddEvent()
        {

        }

        public void RemoveEvent()
        {

        }
        public ICollection<ITimeTableItem> GetContacts()
        {

        }
    }
}