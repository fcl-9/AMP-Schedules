using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.TimeTableItems
{
    public class Lesson : ITimeTableItem
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public User Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Course> Courses { get; set; }
        public string Description { get; set; }

        public bool Editable { get; set; }

        private static int _id;

        /// <summary>
        /// Empty construtor to create data into database.
        /// </summary>
        public Lesson() { }

        /// <summary>
        /// Construtor. Used when data is loaded from the "API" (This data don't need to be persistent).
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="rooms"></param>
        /// <param name="courses"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Lesson(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses,
            string type, string name, string description)
        {
            ID = _id;
            _id++;

            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Type = type;
            Courses = courses;
            Name = name;
            //Teacher = teacher;
        }
    }
}