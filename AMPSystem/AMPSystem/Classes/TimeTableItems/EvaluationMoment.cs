using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class EvaluationMoment : ITimeTableItem
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<Course> Courses { get; set; }

        public string Description { get; set; }

        public EvaluationMoment() { }

        public EvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            ICollection<Course> courses, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
            Name = name;
        }
    }
}