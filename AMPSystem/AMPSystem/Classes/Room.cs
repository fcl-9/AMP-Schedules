using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Number { get; set; }
        public int Floor { get; set; }
        public ICollection<Lesson> Lessons;
    }
}