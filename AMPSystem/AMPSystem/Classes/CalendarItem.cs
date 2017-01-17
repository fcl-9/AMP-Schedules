using System;
using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class CalendarItem
    {
        // The properties are in lowercase because of the Javascript calendar we're using
        // For more information about this properties check FullCalendar Docs at: 
        // https://fullcalendar.io/docs/event_data/Event_Object/

        public virtual int id { get; set; }
        public virtual string title { get; set; }
        public bool allDay { get; set; }
        public virtual DateTime start { get; set; }
        public virtual DateTime end { get; set; }
        public virtual string url { get; set; }
        public virtual string[] className { get; set; }
        public virtual bool editable { get; set; }
        public virtual bool startEditable { get; set; }
        public virtual bool durationEditable { get; set; }
        public virtual bool resourceEditable { get; set; }
        public virtual bool overlap { get; set; }
        public virtual string color { get; set; }
        public virtual string backgroundColor { get; set; }
        public virtual string borderColor { get; set; }
        public virtual string textColor { get; set; }
        public virtual string description { get; set; }
        public virtual string reminder { get; set; }
        public virtual ICollection<Room> rooms { get; set; }
        public virtual string lessonType { get; set; }
        public virtual User teacher { get; set; }
    }
    }