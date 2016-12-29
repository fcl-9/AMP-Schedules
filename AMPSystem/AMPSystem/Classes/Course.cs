using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Course
    {
        public int Id { get; set; }
        public ICollection<int> Years { get; set; }
        public string Name { get; set; }

        private static int _id;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="years"></param>
        public Course(string name, ICollection<int> years)
        {
            Id = _id;
            _id++;

            Name = name;
            Years = years;
        }
    }
}