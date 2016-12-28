using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class User
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        
        public ICollection<Course> Courses { get; set; }

        public User()
        {
            //
        }

        public User(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            Name = name;
            Email = email;
            //Roles = new List<string>();
            Roles = roles;
            Courses = courses;
        }

        
    }
}