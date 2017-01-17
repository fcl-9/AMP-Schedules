using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Course
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="years"></param>
        public Course(int id, string name, ICollection<int> years)
        {
            ExternId = id;
            Name = name;
            Years = years;
        }

        public int Id { get; set; }
        public int ExternId { get; set; }
        public ICollection<int> Years { get; set; }
        public string Name { get; set; }
    }
}