using System;

namespace AMPSystem.Classes
{
    public class TimeTableManager
    {
        public Timetable TimeTable { get; set; }
        public Repository Repository { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timetable"></param>
        /// <param name="repository"></param>
        public TimeTableManager(Timetable timetable, Repository repository)
        {
            TimeTable = timetable;
            Repository = repository;
        }

        /// <summary>
        /// Update time table items to display. For this, get the repository data and add to
        /// ItemList in time table.
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public void UpdateTimeTable(DateTime startDateTime, DateTime endDateTime)
        {
            TimeTable.StartDateTime = startDateTime;
            TimeTable.StartDateTime = endDateTime;

            //Clears the item list
            TimeTable.ItemList.Clear();

            foreach (var item in Repository.Items)
            {
                //Less than zero: t1 is earlier than t2. 
                //Zero: t1 is the same as t2. 
                if (DateTime.Compare(startDateTime, item.StartTime) <= 0 &&
                    DateTime.Compare(endDateTime, item.StartTime) > 0)
                {
                    TimeTable.AddTimetableItem(item);
                }
            }
        }
    }
}
