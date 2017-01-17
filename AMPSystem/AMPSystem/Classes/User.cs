using System.Collections.Generic;

namespace AMPSystem.Classes
{
    public class User
    {
        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="roles"></param>
        /// <param name="courses"></param>
        public User(int id, string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            ExternId = id;
            Name = name;
            Email = email;
            Roles = new List<string>();
            Roles = roles;
            Courses = new List<Course>();
            Courses = courses;
        }

        public User(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            Name = name;
            Email = email;
            Roles = new List<string>();
            Roles = roles;
            Courses = new List<Course>();
            Courses = courses;
        }
        
        public int UserID { get; set; }
        public int ExternId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}