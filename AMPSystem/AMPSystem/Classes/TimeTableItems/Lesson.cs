using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Lesson : ITimeTableItem
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }

        public ICollection<Course> Courses { get; set; }

        public Lesson(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Type = type;
            Courses = courses;
            Name = name;
        }
    }
}