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
