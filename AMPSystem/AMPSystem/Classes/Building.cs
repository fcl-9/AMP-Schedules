using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Building
    {
        public int Id { get; set; }
        public int ExternId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }

        private static int _id;

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="rooms"></param>
        public Building(int id, string name, string address, ICollection<Room> rooms)
        {
            ExternId = id;
            Name = name;
            Address = address;
            Rooms = rooms;
            InformRooms();
        }

        private void InformRooms()
        {
            foreach (var room in Rooms)
            {
                room.Building = this;
            }
        }
    }
}