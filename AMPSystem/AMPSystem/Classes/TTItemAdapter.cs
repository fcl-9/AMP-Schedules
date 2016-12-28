using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TTItemAdapter : ITimeTableItem
    {
        // This class adapts the CalendarItems that have the structure
        // needed to be used by the JS callendar used on the Web Interface
        // to TimeTableItems

        private CalendarItem Item { get; set; }
        public int ItemID { get; set; }
        public string Name { get { return Item.title; } set; }
        public string Color { get { return Item.color; } set; }
        public DateTime StartTime { get { return Item.start; } set; }
        public DateTime EndTime { get { return Item.end; } set; }
        public ICollection<Room> Rooms { get { return Item.rooms; } set; }
        public string Description { get { return Item.description; } set; }
    }
}