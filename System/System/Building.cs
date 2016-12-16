using System.Collections.Generic;

namespace System
{
    public class Building
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}