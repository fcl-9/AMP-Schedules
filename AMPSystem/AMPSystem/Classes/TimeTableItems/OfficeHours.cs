using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.TimeTableItems
{
    public class OfficeHours : ITimeTableItem
    {
        public OfficeHours(int id, string name, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            User teacher)
        {
            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Teacher = teacher;
            Name = name;
            Alerts = new List<Alert>();
        }

        public OfficeHours(int id, string name, DateTime startTime, DateTime endTime, ICollection<Room> rooms,
            User teacher, string color, string reminder)
        {
            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Teacher = teacher;
            Name = name;
            Color = color;
            Alerts = new List<Alert>();
            Reminder = reminder;
        }

        public int ExternId { get; set; }
        public User Teacher { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public bool Editable { get; set; }
        public ICollection<Alert> Alerts { get; set; }
    }
}