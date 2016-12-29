using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;

namespace AMPSystem.Classes
{
    /// <summary>
    /// @Summary not implemented because for now we cannot get this data from the "API".
    /// </summary>
    public class Summary
    {
        public DateTime Day { get; set; }
        public string Content { get; set; }
        public ICollection<Lesson> Lessons;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Summary() { }
    }
}