using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.TimeTableItems
{
    public class EvaluationMoment : ITimeTableItem
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }
        public int ExternId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Course> Courses { get; set; }

        public bool Editable { get; set; }
        public ICollection<Alert> Alerts { get; set; }

        private static int _id;
        
        /// <summary>
        /// Construtor. Used when data is loaded from the "API" (This data don't need to be persistent).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="rooms"></param>
        /// <param name="name"></param>
        /// <param name="courses"></param>
        /// <param name="description"></param>
        public EvaluationMoment(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name, string description)
        {
            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Description = description;
            Alerts = new List<Alert>();
        }

        public EvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name, string description, bool editable, string reminder)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Description = description;
            Alerts = new List<Alert>();
            Editable = editable;
            Reminder = reminder;
        }

        public EvaluationMoment(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name)
        {
            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Alerts = new List<Alert>();
        }

        public EvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name, string description)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Description = description;
            Alerts = new List<Alert>();
        }

        public EvaluationMoment(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, string color,
            ICollection<Course> courses, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Color = color;
            Alerts = new List<Alert>();
        }

        public EvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string name, string color, string description, bool editable)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
            Color = color;
            Description = description;
            Editable = editable;
            Alerts = new List<Alert>();
        }
    }
}