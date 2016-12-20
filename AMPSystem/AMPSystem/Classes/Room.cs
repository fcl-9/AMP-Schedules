using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Number { get; set; }
        public int Floor { get; set; }
        public Lesson Lessons { get; set; }
        public OfficeHours OfficeHours { get; set; }
    }
}