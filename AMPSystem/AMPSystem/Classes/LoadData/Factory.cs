using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;
using Microsoft.Win32;

namespace AMPSystem.Classes
{
    public class Factory
    {
        #region Singleton
    
        private static Factory instance;

        private Factory() { }

        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }
        #endregion

        public ITimeTableItem Create (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher)
        {
            var name = "";
            var i = 0;
            foreach (var course in courses)
            {
                name += course.Name;
                if (courses.Count > 1 && i != courses.Count)
                    name += "/";
                i++;
            }
           return new Lesson(startTime,endTime,rooms,courses,type,name, teacher);
        }

        public ITimeTableItem Create(DateTime startTime , DateTime endTime , ICollection<Room> rooms, User teacher)
        {
            return new OfficeHours(startTime,endTime,rooms, teacher);
        }

        public ITimeTableItem Create(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
        {
            var name = "Avaliação de ";
            var i = 0;
            foreach (var course in courses)
            {
                name += course.Name;
                if (courses.Count > 1 && i != courses.Count)
                    name += "/";
                i++;
            }
            return new EvaluationMoment(startTime, endTime, rooms, courses,name);
        }

  

        public Room CreateRoom(int number, string name, int floor)
        {
            return new Room(number, name, floor);
        }

        public Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return new Course(id, name, years);
        }

        public Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return new Building(id, name, address, rooms);
        }

        public User CreateUser(int id, string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return new User(id, name, email, roles, courses);
        }
    }
}
