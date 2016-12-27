using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class OfficeHours : ITimeTableItem
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public User Teacher { get; set; }
        public string Color { get; set; }

        public OfficeHours(DateTime starTime, DateTime endTime, ICollection<Room> rooms, User teacher)
        {
            StartTime = starTime;
            EndTime = endTime;
            Rooms = rooms;
            Teacher = teacher;
            Name = "Horário de Atendimento de " + teacher.Name;
        }
    }
}
