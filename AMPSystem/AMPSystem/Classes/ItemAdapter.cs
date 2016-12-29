using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class ItemAdapter : CalendarItem
    {
        // This class adapts the TimeTableItems to CalendarItems that have the structure
        // needed to be used by the JS callendar used on the Web Interface
        private ITimeTableItem Item { get; }
        public override DateTime end { get { return Item.EndTime; } }
        public override DateTime start { get { return Item.StartTime; } }
        public override string title { get { return Item.Name; } }
        public override string color { get { return Item.Color; } }
        public override ICollection<Room> rooms { get { return Item.Rooms; } }
        public override string description { get { return Item.Description; } }
        public override User teacher
        {
            get
            {
                if (Item is Lesson)
                {
                    return ((Lesson) Item).Teacher;
                }
                else
                {
                    return null;
                }
            }
        }
        public override string lessonType
        {
            get
            {
                if (Item is Lesson)
                {
                    return ((Lesson)Item).Type;
                }
                else
                {
                    return null;
                }
            }
        }

        public ItemAdapter(ITimeTableItem item)
        {
            Item = item;
        }
    }
}