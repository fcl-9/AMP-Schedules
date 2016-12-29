using System;
using System.Collections.Generic;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.LoadData
{
    public class Factory
    {
        #region Singleton
        private static Factory _instance;

        private Factory() { }

        public static Factory Instance => _instance ?? (_instance = new Factory());
        #endregion

        public ITimeTableItem Create (int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher)
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
           return new Lesson(id, startTime, endTime, rooms, courses, type, name, teacher);
        }

        public ITimeTableItem Create (int id, DateTime startTime , DateTime endTime , ICollection<Room> rooms, User teacher)
        {
            return new OfficeHours(id, startTime,endTime,rooms, teacher);
        }

        public ITimeTableItem Create (int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
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
            return new EvaluationMoment(id, startTime, endTime, rooms, courses, name);
        }
        
        public Room CreateRoom(int id, string name, int floor)
        {
            return new Room(id, name, floor);
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
            return new User(id, name,email,roles,courses);
        }

        public User CreateUser(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return new User(name, email, roles, courses);
        }
    }
}
