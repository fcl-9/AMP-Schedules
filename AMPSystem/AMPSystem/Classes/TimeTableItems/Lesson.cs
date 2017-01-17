using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.TimeTableItems
{
    public class Lesson : ITimeTableItem
    {
        public Lesson(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses,
            string type, string name, User teacher)
        {
            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Type = type;
            Courses = courses;
            Name = name;
            Teacher = teacher;
            Alerts = new List<Alert>();
        }

        public Lesson(int id, DateTime startTime, DateTime endTime, string color, ICollection<Room> rooms,
            ICollection<Course> courses, string type, string name, User teacher, string reminder)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Type = type;
            Courses = courses;
            Name = name;
            Teacher = teacher;
            Color = color;
            Alerts = new List<Alert>();
            Reminder = reminder;
        }

        public int ExternId { get; set; }
        public string Type { get; set; }
        public User Teacher { get; set; }
        public ICollection<Course> Courses { get; set; }
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }

        public bool Editable { get; set; }
        public ICollection<Alert> Alerts { get; set; }
    }
}