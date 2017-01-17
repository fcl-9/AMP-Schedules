using System;
using System.Collections.Generic;

namespace AMPSystem.Models
{
    public class EvaluationMoment
    {
        public int ID { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public string Course { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; }
    }
}