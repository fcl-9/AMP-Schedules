using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class EvaluationMoment : ITimeTableItem
    {
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Day { get; set; }
        public ICollection<Course> Courses { get; set; }

        public EvaluationMoment(DateTime startTime, DateTime endTime, DateTime day, ICollection<Course> courses)
        {
            StarTime = startTime;
            EndTime = endTime;
            Day = day;

            Courses = new List<Course>();
            Courses = courses;
        }
    }
}