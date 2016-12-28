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
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int ID { get; set; }

        public string Name { get; set; }
        public string Color { get; set; }

        public User Teacher { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Room> Rooms { get; set; }
        
        public OfficeHours() { }

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
