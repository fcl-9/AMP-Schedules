using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Building
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }

        public Building(int id, string name, string address, ICollection<Room> rooms)
        {
            ID = id;
            Name = name;
            Address = address;
            Rooms = rooms;
        }
    }
}