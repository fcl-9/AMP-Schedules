using System.Collections.Generic;

namespace AMPSystem.Models
{
    public class Building
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}