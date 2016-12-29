using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;
using Newtonsoft.Json.Linq;

namespace AMPSystem.Classes.LoadData
{
    public class Repository
    {
        public IDataReader DataReader { get; set; }
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
            var data = DataReader.RequestTeachers();
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Teachers"])
            {
                var id = item["ID"].Value<int>();
                var name = item["Name"].Value<string>();
                var email = item["Email"].Value<string>();
                var building = ((List<Building>)Buildings).Find(b => b.ExternId == 1);
                var room = ((List<Room>)building.Rooms).Find(r => r.ExternId == item["Room"].Value<int>());
                var rooms = new List<Room> {room};
                var roles = new List<string> {"Teacher"};

                var courses = new List<Course>();
                foreach (var course in item["Courses"])
                {
                    var mCourse = ((List<Course>)Courses).Find(c => c.ExternId== course.Value<int>());
                    courses.Add(mCourse);
                }

                var teacher = CreateUser(id, name, email, roles, courses);
                foreach (var officeHour in item["OfficeHours"])
                {
                    var officeHourId = officeHour["ID"].Value<int>();
                    var startTime = officeHour["StartTime"].Value<DateTime>();
                    var endTime = officeHour["EndTime"].Value<DateTime>();
                    Items.Add(CreateOfficeHours(officeHourId, startTime, endTime,rooms,teacher));
                }
                Teachers.Add(teacher);
            }
        }

        /// <summary>
        /// Gets all the courses of a user and updates the correspondent list
        /// </summary>
        /// <param name="username">The username of the user</param>
        public void GetUserCourses(string username)
        {
            var data = DataReader.RequestUserCourses(username);
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Courses"])
            {
                var id = item.Value<int>();
                UserCourses.Add(((List<Course>)Courses).Find(c => c.ExternId == id));
            }
        }

        /// <summary>
        /// Gets all the courses from the dataReader and updates the List of courses
        /// </summary>
        public void GetCourses()
        {
            var data = DataReader.RequestCourses();
            if (string.IsNullOrEmpty(data)) return;
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

        /// <summary>
        /// Gets all the rooms and buildings from the dataReader and updates the List of Rooms and Buildings
        /// </summary>
        public void GetRooms()
        {
            var data = DataReader.RequestRooms();
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed)
            {
                var buildingID = item.Value["ID"].Value<int>();
                var buildingName = item.Value["Name"].Value<string>();
                var buildingAddress = item.Value["Address"].Value<string>();

                var rooms = new List<Room>();
                foreach (var room in item.Value["Room"])
                {
                    var roomId = room["ID"].Value<int>();
                    rooms.Add(CreateRoom(roomId, room["Name"].Value<string>(), room["Floor"].Value<int>()));
                }
                Buildings.Add(CreateBuilding(buildingID, buildingName, buildingAddress, rooms));
            }
        }

        /// <summary>
        /// Gets all the lessons/evaluations/officehours from the dataReader and updates the list of items.
        /// that compose the schedule.
        /// </summary>
        /// <param name="username"></param>
        public void GetSchedule(string username)
        {
            var data = DataReader.RequestSchedule(username);
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Schedule"])
            {
                var itemId = item["ID"].Value<int>();
                var startTime = item["StartTime"].Value<DateTime>();
                var endTime = item["EndTime"].Value<DateTime>();
                var lessonType = item["LessonType"].Value<string>();

                var rooms = new List<Room>();
                foreach (var room in item["ClassRoom"])
                {
                    var building = ((List<Building>)Buildings).Find(b => b.ExternId == room["Building"].Value<int>());
                    var mRoom = ((List<Room>) building.Rooms).Find(r => r.ExternId == room["Id"].Value<int>());
                    rooms.Add(mRoom);
                }

                var courses = new List<Course>();
                foreach (var course in item["Courses"])
                {
                    var mCourse = ((List<Course>)Courses).Find(c => c.ExternId == course.Value<int>());
                    courses.Add(mCourse);
                }

                //TODO Comment this.
                if (lessonType == "T" || lessonType == "TP" || lessonType == "PL")
                {
                    var teacher = ((List<User>) Teachers).Find(t => t.ExternId == item["Teacher"].Value<int>());
                    Items.Add(CreateLesson(itemId, startTime, endTime, rooms, courses, lessonType, teacher));
                }
                else
                {
                    Items.Add(CreateEvaluationMoment(itemId, startTime, endTime, rooms, courses));
                }

            }
        }

        #region Calls Factory.
        private static Course CreateCourse(int id, string name, ICollection<int> years)
        {
            return Factory.Instance.CreateCourse(id, name, years);
        }

        private static Building CreateBuilding(int id, string name, string address, ICollection<Room> rooms)
        {
            return Factory.Instance.CreateBuilding(id, name, address, rooms);
        }

        private static Room CreateRoom(int id, string name, int floor)
        {
            return Factory.Instance.CreateRoom(id, name, floor);
        }

        private static User CreateUser (int id, string name, string email, ICollection<string> roles, ICollection<Course> courses)
        {
            return Factory.Instance.CreateUser(id, name, email, roles, courses);
        }

        private static ITimeTableItem CreateLesson (int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, courses, type, teacher);
        }

        private static ITimeTableItem CreateOfficeHours(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, User teacher)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, teacher);
        }

        private static ITimeTableItem CreateEvaluationMoment(int id, DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, courses);
        }
        #endregion
    }
}