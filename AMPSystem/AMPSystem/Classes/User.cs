using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}