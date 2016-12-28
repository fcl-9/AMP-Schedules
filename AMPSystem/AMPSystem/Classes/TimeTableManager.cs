using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TimeTableManager : ISubject, IObserver
    {
        public Timetable TimeTable { get; set; }
        public Repository Repository { get; set; }

        #region Observer Pattern
        public ICollection<IObserver> Observers { get; set; }
        public ICollection<ISubject> Subjects { get; set; }
        #endregion

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
                                TimeTable.AddTimetableItem(item);
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
                                TimeTable.AddTimetableItem(item);
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
                                TimeTable.AddTimetableItem(item);
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
        /// Update time table items to display. For this, get the repository data and add to
        /// ItemList in time table.
        /// </summary>
        /// <param name="subject"></param>
        public void Update(ISubject subject)
        {
            var startDateTime = ((IViewHandler) subject).StartDateTime;
            var endDateTime = ((IViewHandler)subject).EndDateTime;

            TimeTable.StartDateTime = startDateTime;
            TimeTable.StartDateTime = endDateTime;

            //Clears the item list
            TimeTable.ItemList.Clear();

            foreach (var item in Repository.Items)
            {
                //Less than zero: t1 is earlier than t2. 
                //Zero: t1 is the same as t2. 
                if (startDateTime.Date.CompareTo(item.StartTime) <= 0 &&
                    endDateTime.Date.CompareTo(item.StartTime) >= 0)
                {
                    TimeTable.AddTimetableItem(item);
                }
            }
        }
        
        #region Observer Pattern Methods
        /// <summary>
        /// Add observer.
        /// </summary>
        /// <param name="observer"></param>
        public void Add(IObserver observer)
        {
            Observers.Add(observer);
        }

        /// <summary>
        /// Remove observer.
        /// </summary>
        /// <param name="observer"></param>
        public void Remove(IObserver observer)
        {
            Observers.Remove(observer);
        }

        /// <summary>
        /// Notify all observers.
        /// </summary>
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
        #endregion
    }
}
