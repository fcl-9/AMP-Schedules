using System.Collections.Generic;

namespace AMPSystem.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<EvaluationMoment> EvaluationMoments { get; set; }
        public virtual ICollection<OfficeHour> OfficeHours { get; set; }
    }
}