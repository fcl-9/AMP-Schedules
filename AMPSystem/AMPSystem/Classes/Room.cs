using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }

        private static int _id;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="floor"></param>
        public Room(string name, int floor)
        {
            Id = _id;
            _id++;

            Name = name;
            Floor = floor;
        }
    }
}