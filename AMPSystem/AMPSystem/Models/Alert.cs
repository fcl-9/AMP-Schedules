using System;

namespace AMPSystem.Models
{
    public class Alert
    {
        public int ID { get; set; }
        public TimeSpan TimeInterval { get; set; }

        public int? LessonID { get; set; }
        public Lesson Lesson { get; set; }

        public int? EvaluationMomentID { get; set; }
        public EvaluationMoment EvaluationMoment { get; set; }

        public int? OfficeHourID { get; set; }
        public OfficeHour OfficeHour { get; set; }
    }
}