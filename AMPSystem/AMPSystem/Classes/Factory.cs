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
    class Factory
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

        public ITimeTableItem Create (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type)
        {
           return new Lesson(startTime,endTime,rooms,courses,type);
        }

        public ITimeTableItem Create(DateTime startTime , DateTime endTime , ICollection<Room> rooms)
        {
            return new OfficeHours(startTime,endTime,rooms);
        }

        public ITimeTableItem Create(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
        {
            return new EvaluationMoment(startTime, endTime, rooms, courses);
        }

        public ITimeTableItem Create(ITimeTableItem aItem)
        {
            return new ColorDecorator();
        }

        public Room CreateRoom(int number, int floor)
        {
            return new Room(number, floor);
        }

        public Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return new Course(id, name, years);
        }

        public Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return new Building(id, name, address, rooms);
        }

        public User CreateUser(string name, string email, ICollection<string> roles)
        {
            return new User();
        }
    }
}
