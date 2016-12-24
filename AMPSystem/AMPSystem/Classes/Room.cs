using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }

        public Room(int id, string name, int floor)
        {
            Id = id;
            Name = name;
            Floor = floor;
        }
    }
}