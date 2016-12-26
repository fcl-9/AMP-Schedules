using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class ItemAdapter : CalendarItem
    {
        private ITimeTableItem Item { get; set; }

        public override DateTime end { get { return Item.EndTime; } }
        public override DateTime start { get { return Item.StartTime; } }
        public override string title { get { return Item.Name; } }

        public ItemAdapter(ITimeTableItem item)
        {
            Item = item;
            description = "Room(s): <br><ul>";
            foreach (var room in Item.Rooms)
            {
                description += "<li>" + room.Name + "<ul><li> Floor: " + room.Floor + "</li></ul>";
            }
            description += "</ul>";
        }
    }
}