using System;
using System.Collections.Generic;

namespace AMPSystem.Models
{
    public class OfficeHour
    {
        public int ID { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string Reminder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int RoomID { get; set; }
        public virtual Room Room { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; }
    }
}