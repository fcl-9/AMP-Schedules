using System.Collections.Generic;

namespace AMPSystem.Models
{
    public class Room
    {
        public int ID { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }

        public int BuildingID { get; set; }
        public Building Building { get; set; }


        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<EvaluationMoment> EvaluationMoments { get; set; }
        public virtual ICollection<OfficeHour> OfficeHours { get; set; }
    }
}