using System;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TimeTableManager
    {
        public Timetable TimeTable { get; set; }
        public Repository Repository { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public TimeTableManager(Repository repository, DateTime startDateTime, DateTime endDateTime)
        {
            TimeTable = new Timetable(startDateTime, endDateTime);
            Repository = repository;
            //Adds Value to our data class whcih is TimeTable
            foreach (var item in Repository.Items)
            {
                if (startDateTime.Date.CompareTo(item.StartTime) <= 0 &&
                    endDateTime.Date.CompareTo(item.StartTime) >= 0)
                {
                    if (item is Lesson)
                    {
                        // If the lesson belongs to one of the courses that the student assists
                        // then add it to the timetable
                        foreach (var course in ((Lesson) item).Courses)
                        {
                            if (repository.UserCourses.Contains(course))
                            {
                                AddTimetableItem(item);
                            }
                        }
                    }
                    else if (item is EvaluationMoment)
                    {
                        // If the evaluation belongs to one of the courses that the student assists
                        // then add it to the timetable
                        foreach (var course in ((EvaluationMoment) item).Courses)
                        {
                            if (repository.UserCourses.Contains(course))
                            {
                                AddTimetableItem(item);
                            }
                        }
                    }
                    else if (item is OfficeHours)
                    {
                        // If the office hours belongs to one of the teaches that teach one of 
                        // courses that the student assists then add it to the timetable
                        foreach (var course in ((OfficeHours) item).Teacher.Courses)
                        {
                            if (repository.UserCourses.Contains(course))
                            {
                                AddTimetableItem(item);
                            }
                        }
                    }
                }
            }

        }

        //This will only be used to get teachers information don't course data to be added to timetable.
        public TimeTableManager(Repository repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Add a time table item to the list. (Add events)
        /// </summary>
        /// <param name="item"></param>
        public void AddTimetableItem(ITimeTableItem item)
        {
            TimeTable.ItemList.Add(item);
        }

        /// <summary>
        /// Remove a given Time table item from the list.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveTimetableItem(ITimeTableItem item)
        {
            TimeTable.ItemList.Remove(item);
        }

        /// <summary>
        /// Remove a Time table item given a position from the list.
        /// </summary>
        /// <param name="position"></param>
        public void RemoveTimeTableItem(int position)
        {
            TimeTable.ItemList.RemoveAt(position);
        }

        /// <summary>
        /// Returns the lenght of the itemList.
        /// </summary>
        /// <returns></returns>
        public int CountTimeTableItems()
        {
            return TimeTable.ItemList.Count;
        }
    }
}
