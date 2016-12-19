using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Building
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}