using System;
using System.Collections.Generic;
using System.Linq;
using AMPSystem.Interfaces;
using Newtonsoft.Json.Linq;

namespace AMPSystem.Classes
{
    public class Repository
    {
        public DataReader DataReader { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Course> UserCourses { get; set; }
        public ICollection<Building> Buildings { get; set; }
        public ICollection<ITimeTableItem> Items { get; set; }
        public ICollection<User> Teachers { get; set; }

        public Repository()
        {
            Courses = new List<Course>();
            Items = new List<ITimeTableItem>();
            Buildings = new List<Building>();
            UserCourses = new List<Course>();
            Teachers = new List<User>();
        }

        /// <summary>
        /// Gets all the teachers from the dataReader and updates the List of teacher
        /// </summary>
        public void GetTeachers()
        {
            string data = DataReader.RequestTeachers();
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Teachers"])
                {
                    var id = item["ID"].Value<int>();
                    var name = item["Name"].Value<string>();
                    var email = item["Email"].Value<string>();
                    var building = ((List<Building>)Buildings).Find(b => b.ID == 1);
                    var room = ((List<Room>)building.Rooms).Find(r => r.Id == item["Room"].Value<int>());
                    var rooms = new List<Room>();
                    rooms.Add(room);
                    var roles = new List<string>();
                    roles.Add("Teacher");
                    var courses = new List<Course>();
                    foreach (var course in item["Courses"])
                    {
                        var mCourse = ((List<Course>)Courses).Find(c => c.Id== course.Value<int>());
                        courses.Add(mCourse);
                    }
                    var teacher = CreateUser(id, name, email, roles, courses);
                    foreach (var officeHour in item["OfficeHours"])
                    {
                        var startTime = officeHour["StartTime"].Value<DateTime>();
                        var endTime = officeHour["EndTime"].Value<DateTime>();
                        Items.Add(CreateOfficeHours(startTime, endTime,rooms,teacher));
                    }
                    Teachers.Add(teacher);
                }
            }
        }

        /// <summary>
        /// Gets all the courses of a user and updates the correspondent list
        /// </summary>
        /// <param name="username">The username of the user</param>
        public void GetUserCourses(string username)
        {
            string data = DataReader.RequestUserCourses(username);
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Courses"])
                {
                    var id = item.Value<int>();
                    UserCourses.Add(((List<Course>)Courses).Find(c => c.Id == item.Value<int>()));
                }
            }
        }

        /// <summary>
        /// Gets all the courses from the dataReader and updates the List of courses
        /// </summary>
        public void GetCourses()
        {
            string data = DataReader.RequestCourses();
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Courses"])
                {
                    var id = item["ID"].Value<int>();
                    var name = item["Name"].Value<string>();
                    var years = new List<int>();
                    foreach (var year in item["Year"])
                    {
                        years.Add(year.Value<int>());
                    }
                    Courses.Add(CreateCourse(id, name, years));
                }
            }
        }

        /// <summary>
        /// Get's all the rooms and buildings from the dataReader and updates the List of Rooms and Buildings
        /// </summary>
        public void GetRooms()
        {
            string data = DataReader.RequestRooms();
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed)
                {
                    var buildingID = item.Value["ID"].Value<int>();
                    var buildingName = item.Value["Name"].Value<string>();
                    var buildingAddress = item.Value["Address"].Value<string>();
                    var rooms = new List<Room>();
                    foreach (var room in item.Value["Room"])
                    {
                        rooms.Add(CreateRoom(room["ID"].Value<int>(), room["Name"].Value<string>(), room["Floor"].Value<int>()));
                    }
                    Buildings.Add(CreateBuilding(buildingID, buildingName, buildingAddress, rooms));
                }
            }
        }

        /// <summary>
        /// Get's all the lessons/evaluations/officehours from the dataReader and updates the List of items
        /// that compose the schedule.
        /// </summary>
        /// <param name="path">The username of the user</param>
        public void GetSchedule(string username)
        {
            string data = DataReader.RequestSchedule(username);
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Schedule"])
                {
                    var startTime = item["StartTime"].Value<DateTime>();
                    var endTime = item["EndTime"].Value<DateTime>();
                    var lessonType = item["LessonType"].Value<string>();
                    var rooms = new List<Room>();
                    foreach (var room in item["ClassRoom"])
                    {
                        var building = ((List<Building>)Buildings).Find(b => b.ID == room["Building"].Value<int>());
                        var mRoom = ((List<Room>) building.Rooms).Find(r => r.Id == room["Id"].Value<int>());
                        rooms.Add(mRoom);
                    }
                    var courses = new List<Course>();
                    foreach (var course in item["Courses"])
                    {
                        var mCourse = ((List<Course>)Courses).Find(c => c.Id == course.Value<int>());
                        courses.Add(mCourse);
                    }
                    if (lessonType == "T" || lessonType == "TP" || lessonType == "PL")
                    {
                        var teacher = ((List<User>)Teachers).Find(t => t.UserID == item["Teacher"].Value<int>());
                        Items.Add(CreateLesson(startTime, endTime, rooms, courses, lessonType, teacher));
                    }
                    else
                    {
                        Items.Add(CreateEvaluationMoment(startTime, endTime, rooms, courses));
                    }

                }
            }
        }

        // Methods that invoke the factory to create objects
        private Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return Factory.Instance.CreateCourse(id, name, years);
        }

        private Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return Factory.Instance.CreateBuilding(id, name, address, rooms);
        }

        private Room CreateRoom(int number, string name, int floor)
        {
            return Factory.Instance.CreateRoom(number, name, floor);
        }

        private User CreateUser (int id, string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return Factory.Instance.CreateUser(id, name, email, roles, courses);
        }

        private ITimeTableItem CreateLesson (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher)
        {
            return Factory.Instance.Create(startTime, endTime, rooms, courses, type, teacher);
        }

        private ITimeTableItem CreateOfficeHours(DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher)
        {
            return Factory.Instance.Create(startTime, endTime, rooms, teacher);
        }
        private ITimeTableItem CreateEvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
        {
            return Factory.Instance.Create(startTime, endTime, rooms, courses);
        }
    }
}