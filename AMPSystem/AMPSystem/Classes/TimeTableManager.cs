using System;
using System.Collections.Generic;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TimeTableManager
    {
        #region Singleton

        private static TimeTableManager _instance;

        /// <summary>
        ///     Constructor.
        /// </summary>
        private TimeTableManager()
        {
            Repository = Repository.Instance;
        }

        #endregion

        public Timetable TimeTable { get; set; }
        public Repository Repository { get; set; }

        public static TimeTableManager Instance => _instance ?? (_instance = new TimeTableManager());

        /// <summary>
        ///     Create the time table.
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="currentUser"></param>
        public void CreateTimeTable(DateTime startDateTime, DateTime endDateTime, User currentUser)
        {
            TimeTable = new Timetable(startDateTime, endDateTime, currentUser);
            foreach (var item in Repository.Items)
            {
                if ((DateTime.Compare(startDateTime, item.StartTime) > 0) ||
                    (DateTime.Compare(endDateTime, item.StartTime) < 0)) continue;

                foreach (var course in GetCourses(item))
                {
                    if (!Repository.UserCourses.Contains(course)) continue;
                    if (!((List<ITimeTableItem>) TimeTable.ItemList).Exists(
                        i =>
                            (i.StartTime == item.StartTime) && (i.EndTime == item.EndTime) &&
                            (i.Name == item.Name)))
                        AddTimetableItem(item);
                }
            }
        }

        /// <summary>
        ///     Return courses list in accordance with the type.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static ICollection<Course> GetCourses(ITimeTableItem item)
        {
            if (item is Lesson) return ((Lesson) item).Courses;
            if (item is EvaluationMoment) return ((EvaluationMoment) item).Courses;
            return ((OfficeHours) item).Teacher.Courses;
        }

        /// <summary>
        ///     Add a time table item to the list. (Add events)
        /// </summary>
        /// <param name="item"></param>
        public void AddTimetableItem(ITimeTableItem item)
        {
            TimeTable.ItemList.Add(item);
        }

        /// <summary>
        ///     Remove a given Time table item from the list.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveTimeTableItem(ITimeTableItem item)
        {
            TimeTable.ItemList.Remove(item);
        }

        /// <summary>
        ///     Remove a Time table item given a position from the list.
        /// </summary>
        /// <param name="position"></param>
        public void RemoveTimeTableItem(int position)
        {
            TimeTable.ItemList.RemoveAt(position);
        }

        /// <summary>
        ///     Returns the lenght of the itemList.
        /// </summary>
        /// <returns></returns>
        public int CountTimeTableItems()
        {
            return TimeTable.ItemList.Count;
        }
        
    }
}