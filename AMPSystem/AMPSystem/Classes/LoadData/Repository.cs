using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using AMPSystem.Models;
using Newtonsoft.Json.Linq;
using EvaluationMoment = AMPSystem.Models.EvaluationMoment;
using Lesson = AMPSystem.Models.Lesson;

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
        public bool DataLoaded { get; private set; }

        public void GetData(MailAddress user)
        {
            var dbUser = DbManager.Instance.ReturnUserIfExists(user.Address);
            LoadAllCourses();
            LoadRooms();
            LoadTeachers(dbUser);
            LoadUserCourses(user.User);
            LoadSchedule(user.User, dbUser);

            if (dbUser != null) AddCustomEvents(dbUser);
            DataLoaded = true;
        }

        public void CleanRepository()
        {
            Courses.Clear();
            Buildings.Clear();
            Teachers.Clear();
            UserCourses.Clear();
            Items.Clear();
            DataLoaded = false;
        }

        private ICollection<Course> LoadCourses(JToken item)
        {
            return
                item["Courses"].Select(course => ((List<Course>) Courses).Find(c => c.ExternId == course.Value<int>()))
                    .ToList();
        }

        private void LoadOfficeHours(JToken item, Models.User dbUser, ICollection<Room> rooms, User teacher)
        {
            foreach (var officeHour in item["OfficeHours"])
            {
                var officeHourId = officeHour["ID"].Value<int>();
                var startTime = officeHour["StartTime"].Value<DateTime>();
                var endTime = officeHour["EndTime"].Value<DateTime>();
                var officeHourName = GenerateOfficeHourName(teacher.Name);
                var office = DbManager.Instance.ReturnOfficeHourIfExists(officeHourName, startTime, endTime, dbUser);

                if (office != null)
                {
                    var mItem = CreateOfficeHours(officeHourId, officeHourName, startTime, endTime, rooms, teacher,
                        office.Color);
                    Items.Add(mItem);
                    AddAlertsToOfficeHour(office, (OfficeHours) mItem);
                    continue;
                }
                Items.Add(CreateOfficeHours(officeHourId, officeHourName, startTime, endTime, rooms, teacher));
            }
        }

        private void LoadUserCourses(string userName)
        {
            var data = DataReader.RequestUserCourses(userName);
            if (string.IsNullOrEmpty(data)) return;

            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Courses"])
            {
                var id = item.Value<int>();
                UserCourses.Add(((List<Course>) Courses).Find(c => c.ExternId == id));
            }
        }

        private void LoadAllCourses()
        {
            var data = DataReader.RequestCourses();
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Courses"])
            {
                var id = item["ID"].Value<int>();
                var name = item["Name"].Value<string>();
                var years = item["Year"].Select(year => year.Value<int>()).ToList();
                Courses.Add(CreateCourse(id, name, years));
            }
        }

        private void LoadRooms()
        {
            var data = DataReader.RequestRooms();
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed)
            {
                var buildingId = item.Value["ID"].Value<int>();
                var buildingName = item.Value["Name"].Value<string>();
                var buildingAddress = item.Value["Address"].Value<string>();

                var rooms = (from room in item.Value["Room"]
                    let roomId = room["ID"].Value<int>()
                    select CreateRoom(roomId, room["Name"].Value<string>(), room["Floor"].Value<int>())).ToList();
                Buildings.Add(CreateBuilding(buildingId, buildingName, buildingAddress, rooms));
            }
        }

        private void LoadTeachers(Models.User dbUser)
        {
            var data = DataReader.RequestTeachers();
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Teachers"])
            {
                var id = item["ID"].Value<int>();
                var name = item["Name"].Value<string>();
                var email = item["Email"].Value<string>();
                var building = ((List<Building>) Buildings).Find(b => b.ExternId == 1);
                var rooms = new List<Room>
                {
                    ((List<Room>) building.Rooms).Find(r => r.ExternId == item["Room"].Value<int>())
                };
                var roles = new List<string> {"Teacher"};
                var teacher = CreateUser(id, name, email, roles, LoadCourses(item));
                Teachers.Add(teacher);
                LoadOfficeHours(item, dbUser, rooms, teacher);
            }
        }

        private void LoadSchedule(string username, Models.User dbUser)
        {
            var data = DataReader.RequestSchedule(username);
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Schedule"])
            {
                var itemId = item["ID"].Value<int>();
                var startTime = item["StartTime"].Value<DateTime>();
                var endTime = item["EndTime"].Value<DateTime>();
                var type = item["LessonType"].Value<string>();
                var rooms = (from room in item["ClassRoom"]
                    let building = ((List<Building>) Buildings).Find(b => b.ExternId == room["Building"].Value<int>())
                    select ((List<Room>) building.Rooms).Find(r => r.ExternId == room["Id"].Value<int>())).ToList();
                var courses = (IList<Course>) LoadCourses(item);

                if (string.IsNullOrEmpty(type))
                    LoadEvaluation(courses, startTime, endTime, dbUser, itemId, rooms);
                else
                    LoadLesson(item, courses, startTime, endTime, dbUser, itemId, type, rooms);
            }
        }

        private void LoadLesson(JToken item, IList<Course> courses, DateTime startTime, DateTime endTime,
            Models.User dbUser, int id, string type, ICollection<Room> rooms)
        {
            var teacher = ((List<User>) Teachers).Find(t => t.ExternId == item["Teacher"].Value<int>());
            var name = GenerateLessonName(courses);
            var mLesson = DbManager.Instance.ReturnLessonIfExists(name, startTime, endTime, dbUser);
            if (mLesson != null)
            {
                var mItem = CreateLesson(id, name, startTime, endTime, rooms, courses, type, teacher,
                    mLesson.Color);
                Items.Add(mItem);
                AddAlertsToLesson(mLesson, (TimeTableItems.Lesson) mItem);
                return;
            }
            Items.Add(CreateLesson(id, name, startTime, endTime, rooms, courses, type, teacher));
        }

        private void LoadEvaluation(IList<Course> courses, DateTime startTime, DateTime endTime, Models.User dbUser,
            int id, ICollection<Room> rooms)
        {
            var name = GenerateEvaluationName(courses);
            var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(name, startTime, endTime, dbUser);
            if (mEvaluation != null)
            {
                var mItem = CreateEvaluationMoment(id, startTime, endTime, rooms, courses, name,
                    mEvaluation.Color);
                Items.Add(mItem);
                AddAlertsToEvaluation(mEvaluation, (TimeTableItems.EvaluationMoment) mItem);
                return;
            }
            Items.Add(CreateEvaluationMoment(id, startTime, endTime, rooms, courses, name));
        }

        /// <summary>
        ///     Checks DB to see if exists any events that weren't load by DataReader.
        ///     If so they are custom events and this method adds the events to the items collection.
        /// </summary>
        private void AddCustomEvents(Models.User user)
        {
            List<ITimeTableItem> knownEvaluations = null;
            try
            {
                knownEvaluations = ((List<ITimeTableItem>) Items).FindAll(i => i is TimeTableItems.EvaluationMoment);
            }
            catch (ArgumentNullException e)
            {
            }

            foreach (var dbEvMoment in DbManager.Instance.EvaluationMoments(user))
            {
                var knownEv = knownEvaluations?.FirstOrDefault(
                    e =>
                        (e.Name == dbEvMoment.Name) && (e.StartTime == dbEvMoment.StartTime) &&
                        (e.EndTime == dbEvMoment.EndTime));

                if (knownEv != null) return;
                var rooms = (from room in dbEvMoment.Rooms
                        let building = ((List<Building>) Buildings).FirstOrDefault(b => b.Name == room.Building.Name)
                        select
                        ((List<Room>) building.Rooms).FirstOrDefault(
                            r => (r.Name == room.Name) && (r.Floor == room.Floor)))
                    .ToList();
                var courses = new List<Course> {Courses.FirstOrDefault(c => c.Name == dbEvMoment.Course)};
                var mItem = CreateEvaluationMoment(dbEvMoment.StartTime, dbEvMoment.EndTime, rooms, courses,
                    dbEvMoment.Name, dbEvMoment.Color, dbEvMoment.Description, true);
                Items.Add(mItem);
                AddAlertsToEvaluation(dbEvMoment, (TimeTableItems.EvaluationMoment) mItem);
            }
        }

        private static void AddAlertsToLesson(Lesson dbLesson, ITimeTableItem lesson)
        {
            foreach (var alert in dbLesson.Alerts)
                new Alert(alert.ID, alert.AlertTime, lesson);
        }

        private static void AddAlertsToOfficeHour(OfficeHour dbOfficeHour, ITimeTableItem officeHours)
        {
            foreach (var alert in dbOfficeHour.Alerts)
                new Alert(alert.ID, alert.AlertTime, officeHours);
        }

        private static void AddAlertsToEvaluation(EvaluationMoment dbEvaluation, ITimeTableItem evaluation)
        {
            foreach (var alert in dbEvaluation.Alerts)
                new Alert(alert.ID, alert.AlertTime, evaluation);
        }

        private static string GenerateLessonName(IList<Course> courses)
        {
            var name = "";
            for (var i = 0; i < courses.Count - 1; i++)
                name += courses[i].Name + "/";
            return name += courses[courses.Count - 1];
        }

        private static string GenerateEvaluationName(IList<Course> courses)
        {
            var name = "Avaliação de ";
            for (var i = 0; i < courses.Count - 1; i++)
                name += courses[i].Name + "/";
            return name += courses[courses.Count - 1];
        }

        private static string GenerateOfficeHourName(string teacherName)
        {
            return "Horário de Atendimento de " + teacherName;
        }

        private static ITimeTableItem CreateEvaluationMoment(DateTime startTime, DateTime endTime,
            ICollection<Room> rooms,
            ICollection<Course> courses, string name, string color, string description, bool editable)
        {
            return Factory.Instance.Create(startTime, endTime, rooms, courses, name, color, description, editable);
        }

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

        private static User CreateUser(int id, string name, string email, ICollection<string> roles,
            ICollection<Course> courses)
        {
            return Factory.Instance.CreateUser(id, name, email, roles, courses);
        }

        private static ITimeTableItem CreateLesson(int id, string name, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher)
        {
            return Factory.Instance.Create(id, name, startTime, endTime, rooms, courses, type, teacher);
        }

        private static ITimeTableItem CreateLesson(int id, string name, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, ICollection<Course> courses, string type, User teacher, string color)
        {
            return Factory.Instance.Create(id, name, color, startTime, endTime, rooms, courses, type, teacher);
        }

        private static ITimeTableItem CreateOfficeHours(int id, string name, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, User teacher)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, teacher, name);
        }

        private static ITimeTableItem CreateOfficeHours(int id, string name, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, User teacher, string color)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, teacher, name, color);
        }

        private static ITimeTableItem CreateEvaluationMoment(int id, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, ICollection<Course> courses, string name)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, courses, name);
        }

        private static ITimeTableItem CreateEvaluationMoment(int id, DateTime startTime, DateTime endTime,
            ICollection<Room> rooms, ICollection<Course> courses, string name, string color)
        {
            return Factory.Instance.Create(id, startTime, endTime, rooms, courses, name, color);
        }

        #region Singleton
        private static Repository _instance;

        private Repository()
        {
            Courses = new List<Course>();
            Items = new List<ITimeTableItem>();
            Buildings = new List<Building>();
            UserCourses = new List<Course>();
            Teachers = new List<User>();
        }
        
        public static Repository Instance => _instance ?? (_instance = new Repository());
        #endregion
    }
}