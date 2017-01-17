using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;

namespace AMPSystem.Classes
{
    /// <summary>
    ///     @Summary isn't implemented because we cannot get this data from the "API".
    /// </summary>
    public class Summary
    {
        public ICollection<Lesson> Lessons;

        public DateTime Day { get; set; }
        public string Content { get; set; }
    }
}