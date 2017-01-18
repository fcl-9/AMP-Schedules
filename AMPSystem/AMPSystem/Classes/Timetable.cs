using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Timetable
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public Timetable(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            ItemList = new List<ITimeTableItem>();
        }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public IList<ITimeTableItem> ItemList { get; set; }
    }
}