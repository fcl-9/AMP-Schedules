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

        public ICollection<Building> Buildings { get; set; }

        public ICollection<ITimeTableItem> Items { get; set; }

        public ICollection<User> Teachers { get; set; }

        public Repository()
        {
            Courses = new List<Course>();
            Items = new List<ITimeTableItem>();
            Buildings = new List<Building>();
        }

        /// <summary>
        /// Get's all the courses from the dataReader and updates the List of courses
        /// </summary>
        /// <param name="path">The path of file that needs to be read to create the list</param>
        public void GetTeachers(string path)
        {
            string data = DataReader.RequestData(path);
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Teachers"])
                {
                    var id = item["ID"].Value<int>();
                    var name = item["Name"].Value<string>();
                    var email = item["Email"].Value<string>();
                    var building = ((List<Building>)Buildings).Find(b => b.ID == 1);
                    var room = ((List<Room>)building.Rooms).Find(r => r.Number == item["Room"].Value<int>());
                    var rooms = new List<Room>();
                    rooms.Add(room);
                    var roles = new List<string>();
                    roles.Add("Teacher");
                    var courses = new List<Course>();
                    foreach (var course in item["Courses"])
                    {
                        var mCourse = ((List<Course>)Courses).Find(c => c.ID== course.Value<int>());
                        courses.Add(mCourse);
                    }
                    foreach (var officeHour in item["OfficeHours"])
                    {
                        var startTime = item["StartTime"].Value<DateTime>();
                        var endTime = item["EndTime"].Value<DateTime>();
                        Items.Add(CreateOfficeHours(startTime, endTime,rooms));
                    }
                    CreateUser(name, email, roles);
                }
            }
        }

        /// <summary>
        /// Get's all the courses from the dataReader and updates the List of courses
        /// </summary>
        /// <param name="path">The path of file that needs to be read to create the list</param>
        public void GetCourses(string path)
        {
            string data = DataReader.RequestData(path);
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
        /// Get's all the courses from the dataReader and updates the List of Rooms and Buildings
        /// </summary>
        /// <param name="path">The path of file that needs to be read to create the lists</param>
        public void GetRooms(string path)
        {
            string data = DataReader.RequestData(path);
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
                        rooms.Add(CreateRoom(room["Number"].Value<int>(), room["Floor"].Value<int>()));
                    }
                    Buildings.Add(CreateBuilding(buildingID, buildingName, buildingAddress, rooms));
                }
            }
        }

        /// <summary>
        /// Get's all the lessons/evaluations/officehours from the dataReader and updates the List of items
        /// that compose the schedule.
        /// </summary>
        /// <param name="path">The path of file that needs to be read to create the list</param>
        public void GetSchedule(string path)
        {
            string data = DataReader.RequestData(path);
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
                        var mRoom = ((List<Room>) building.Rooms).Find(r => r.Number == room["Number"].Value<int>());
                        rooms.Add(mRoom);
                    }
                    var courses = new List<Course>();
                    foreach (var course in item["Courses"])
                    {
                        var mCourse = ((List<Course>)Courses).Find(c => c.ID == course.Value<int>());
                        courses.Add(mCourse);
                    }
                    if (lessonType == "T" || lessonType == "TP" || lessonType == "PL")
                    {
                        Items.Add(CreateLesson(startTime, endTime, rooms, courses, lessonType));
                    }
                    else
                    {
                        Items.Add(CreateEvaluationMoment(startTime, endTime, rooms, courses));
                    }

                }
            }
        }

        private Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return Factory.Instance.CreateCourse(id, name, years);
        }

        private Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return Factory.Instance.CreateBuilding(id, name, address, rooms);
        }

        private Room CreateRoom(int number, int floor)
        {
            return Factory.Instance.CreateRoom(number, floor);
        }

        private User CreateUser (string name, string email, ICollection<string> roles)
        {
            return Factory.Instance.CreateUser(name, email, roles);
        }

        private ITimeTableItem CreateLesson (DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type)
        {
            return Factory.Instance.Create(startTime,endTime,rooms,courses,type);
        }

        private ITimeTableItem CreateOfficeHours(DateTime startTime, DateTime endTime, ICollection<Room> rooms)
        {
            return Factory.Instance.Create(startTime, endTime, rooms);
        }
        private ITimeTableItem CreateEvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
        {
            return Factory.Instance.Create(startTime, endTime, rooms, courses);
        }
    }
}