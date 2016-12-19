using System.Collections.Generic;

namespace System
{
    public class Summary
    {
        public DateTime Day { get; set; }
        public string Content { get; set; }
        public ICollection<Lesson> Lessons;

    }
}