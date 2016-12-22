using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Course
    {
        public int ID { get; set; }
        public ICollection<int> Years { get; set; }
        public string Name { get; set; }

        public Course(int id, string name, ICollection<int> years)
        {
            ID = id;
            Years = years;
            Name = name;
        }
    }
}