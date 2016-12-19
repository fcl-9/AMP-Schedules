using System;
using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class Summary
    {
        public DateTime Day { get; set; }
        public string Content { get; set; }
        public ICollection<Lesson> Lessons;

    }
}