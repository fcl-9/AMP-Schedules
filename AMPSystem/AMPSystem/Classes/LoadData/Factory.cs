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

        public ITimeTableItem Create (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type)
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
           return new Lesson(startTime, endTime, rooms, courses, type, name, "");
        }

        public ITimeTableItem Create (DateTime startTime , DateTime endTime , ICollection<Room> rooms, User teacher)
        {
            return new OfficeHours(startTime,endTime,rooms, teacher,"");
        }

        public ITimeTableItem Create (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
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
            return new EvaluationMoment(startTime, endTime, rooms, courses, name,"");
        }
        
        public Room CreateRoom(string name, int floor)
        {
            return new Room(name, floor);
        }

        public Course CreateCourse(string name, ICollection<int> years)
        {
            return new Course(name, years);
        }

        public Building CreateBuilding(string name, string address, ICollection<Room> rooms)
        {
            return new Building(name, address, rooms);
        }

        public User CreateUser(string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return new User(name,email,roles,courses);
        }
    }
}
