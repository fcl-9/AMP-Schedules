using System;

namespace AMPSystem.Classes
{
    public class CalendarItem
    {
        // The properties are in lowercase because of the Javascript calendar we're using
        public int id { get; set; }
        public virtual string title { get; set; }
        public bool allDay { get; set; }
        public virtual DateTime start { get; set; }
        public virtual DateTime end { get; set; }
        public string url { get; set; }
        public string[] className { get; set; }
        public bool editable { get; set; }
        public bool startEditable { get; set; }
        public bool durationEditable { get; set; }
        public bool resourceEditable { get; set; }
        public bool overlap { get; set; }
        public string color { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }
        public string description { get; set; }
        }
    }