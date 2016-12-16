using System.Collections.Generic;

namespace System
{
    public abstract class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}