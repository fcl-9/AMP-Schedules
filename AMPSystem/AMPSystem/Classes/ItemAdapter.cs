using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public sealed class ItemAdapter : CalendarItem
    {
        // This class adapts the TimeTableItems to CalendarItems that have the structure
        // needed to be used by the JS callendar used on the Web Interface.
        private ITimeTableItem Item { get; }
        public override DateTime end { get { return Item.EndTime; } }
        public override DateTime start { get { return Item.StartTime; } }
        public override string title { get { return Item.Name; } }
        public override string color { get { return Item.Color; } }

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