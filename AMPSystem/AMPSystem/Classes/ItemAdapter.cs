using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class ItemAdapter : CalendarItem
    {
        /// <summary>
        ///     Item adapter.
        /// </summary>
        /// <param name="item"></param>
        public ItemAdapter(ITimeTableItem item)
        {
            Item = item;
        }

        /// <summary>
        ///     This class adapts the TimeTableItems to CalendarItems that have the structure
        ///     needed to be used by the JS callendar used on the Web Interface
        /// </summary>
        private ITimeTableItem Item { get; }

        public override int id => Item.ID;

        public override DateTime end => Item.EndTime;
        public override DateTime start => Item.StartTime;

        public override string title => Item.Name;
        public override string color => Item.Color;
        public override string description => Item.Description;
        public override string reminder => Item.Reminder;
        public override string lessonType => (Item as Lesson)?.Type;

        public override bool editable => Item.Editable;

        public override ICollection<Room> rooms => Item.Rooms;

        public override User teacher => (Item as Lesson)?.Teacher;
    }
}