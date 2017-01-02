using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Room
    {
        public int Id { get; set; }
        public int ExternId { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public Building Building { get; set; }

        private static int _id;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="floor"></param>
        /// <param name="building"></param>
        public Room(int id, string name, int floor)
        {
            ExternId = id;
            Name = name;
            Floor = floor;
        }
    }
}