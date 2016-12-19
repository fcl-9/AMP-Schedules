using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Lesson : ITimeTableItem
    {
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Day { get; set; }
        public string Type { get; set; }
        public ICollection<Course> Courses { get; set; }

        public Lesson(DateTime startTime, DateTime endTime, DateTime day, ICollection<Course> courses, string type)
        {
            StarTime = startTime;
            EndTime = endTime;
            Day = day;
            Type = type;

            Courses = new List<Course>();
            Courses = courses;
        }
    }
}