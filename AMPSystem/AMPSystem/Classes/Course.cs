using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Course
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<EvaluationMoment> EvaluationMoments { get; set; }

        public Course(int year, string name, ICollection<User> users, ICollection<EvaluationMoment> evaluationMoments)
        {
            Year = year;
            Name = name;

            Users = new List<User>();
            Users = users;
        }
    }
}