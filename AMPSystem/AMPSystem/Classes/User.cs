using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<Course> Courses { get; set; }

        public User(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            Name = name;
            Email = email;
            Roles = roles;
            Courses = courses;
        }
    }
}