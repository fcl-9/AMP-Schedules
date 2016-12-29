using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public Building Building { get; set; }

        private static int _id;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="floor"></param>
        /// <param name="building"></param>
        public Room(string name, int floor, Building building)
        {
            Id = _id;
            _id++;

            Name = name;
            Floor = floor;
            Building = building;
        }
    }
}