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

        public OfficeHours(DateTime starTime, DateTime endTime, ICollection<Room> rooms)
        {
            StartTime = starTime;
            EndTime = endTime;
            Rooms = rooms;
        }
    }
}
