using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Timetable
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public User CurrentUser { get; set; }

        public IList<ITimeTableItem> ItemList { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public Timetable(DateTime startDateTime, DateTime endDateTime, User currentUser)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            CurrentUser = currentUser;
            ItemList = new List<ITimeTableItem>();
        }
    }
}
