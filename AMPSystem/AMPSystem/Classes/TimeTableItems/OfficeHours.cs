using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.TimeTableItems
{
    public class OfficeHours : ITimeTableItem
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }
        public int ExternId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public User Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }

        private static int _id;

        public bool Editable { get; set; }

        /// <summary>
        /// Empty construtor to create data into database.
        /// </summary>
        public OfficeHours() { }

        /// <summary>
        /// Construtor. Used when data is loaded from the "API" (This data don't need to be persistent).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="rooms"></param>
        /// <param name="teacher"></param>
        /// <param name="description"></param>
        public OfficeHours(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher, string description)
        {
            ID = _id;
            _id++;

            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Teacher = teacher;
            Name = "Horário de Atendimento de " + teacher.Name;
            Description = description;
        }

        public OfficeHours(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher)
        {
            ID = _id;
            _id++;

            ExternId = id;
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Teacher = teacher;
            Name = "Horário de Atendimento de " + teacher.Name;
        }
    }
}
