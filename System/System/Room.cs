using System.Collections.Generic;

namespace System
{
    public class Room
    {
        public int Number { get; set; }
        public int Floor { get; set; }
        public ICollection<Lesson> Lessons;
    }
}