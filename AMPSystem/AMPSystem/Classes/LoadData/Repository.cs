using System;
using System.Collections.Generic;
using System.Linq;
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
        private AmpDbContext _db = new AmpDbContext();
       
        #region Singleton
        private static Repository _instance;

        public Repository()
        {
            Courses = new List<Course>();
            Items = new List<ITimeTableItem>();
            Buildings = new List<Building>();
            UserCourses = new List<Course>();
            Teachers = new List<User>();
        }


        public static Repository Instance => _instance ?? (_instance = new Repository());
        #endregion

        public IDataReader DataReader { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Course> UserCourses { get; set; }
        public ICollection<Building> Buildings { get; set; }
        public ICollection<ITimeTableItem> Items { get; set; }
        public ICollection<User> Teachers { get; set; }
        public bool DataLoaded { get; private set; }

        public void GetData(string user)
        {
            GetCourses();
            GetRooms();
            GetTeachers();

            GetUserCourses(user);
            GetSchedule(user);

            AddCustomEvents();
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

        /// <summary>
        ///     Gets all the teachers from the dataReader and updates the List of teacher
        /// </summary>
        private void GetTeachers()
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
                var room = ((List<Room>) building.Rooms).Find(r => r.ExternId == item["Room"].Value<int>());
                var rooms = new List<Room> {room};
                var roles = new List<string> {"Teacher"};

                var courses = new List<Course>();
                foreach (var course in item["Courses"])
                {
                    var mCourse = ((List<Course>) Courses).Find(c => c.ExternId == course.Value<int>());
                    courses.Add(mCourse);
                }

                var teacher = CreateUser(id, name, email, roles, courses);
                foreach (var officeHour in item["OfficeHours"])
                {
                    var officeHourId = officeHour["ID"].Value<int>();
                    var startTime = officeHour["StartTime"].Value<DateTime>();
                    var endTime = officeHour["EndTime"].Value<DateTime>();

                    var mName = GenerateOfficeHourName(teacher.Name);
                    var mOfficeHour = DbManager.Instance.ReturnOfficeHourIfExists(mName, startTime, endTime);
                    if (mOfficeHour == null)
                    {
                        Items.Add(CreateOfficeHours(officeHourId, mName, startTime, endTime, rooms, teacher));
                    }
                    else
                    {
                        var mItem = CreateOfficeHours(officeHourId, mName, startTime, endTime, rooms, teacher,
                            mOfficeHour.Color);
                        Items.Add(mItem);
                        AddAlertsToOfficeHour(mOfficeHour, (OfficeHours) mItem);
                    }
                }
                Teachers.Add(teacher);
            }
        }

        /// <summary>
        ///     Gets all the courses of a user and updates the correspondent list
        /// </summary>
        /// <param name="username">The username of the user</param>
        private void GetUserCourses(string username)
        {
            var data = DataReader.RequestUserCourses(username);
            if (string.IsNullOrEmpty(data)) return;
            var dataParsed = JObject.Parse(data);
            foreach (var item in dataParsed["Courses"])
            {
                var id = item.Value<int>();
                UserCourses.Add(((List<Course>) Courses).Find(c => c.ExternId == id));
            }
        }

        /// <summary>
        ///     Gets all the courses from the dataReader and updates the List of courses
        /// </summary>
        private void GetCourses()
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
                    years.Add(year.Value<int>());
                Courses.Add(CreateCourse(id, name, years));
            }
        }

        /// <summary>
        ///     Gets all the rooms and buildings from the dataReader and updates the List of Rooms and Buildings
        /// </summary>
        private void GetRooms()
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
        ///     Gets all the lessons/evaluations/officehours from the dataReader and updates the list of items.
        ///     that compose the schedule.
        /// </summary>
        /// <param name="username"></param>
        private void GetSchedule(string username)
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
                    var building = ((List<Building>) Buildings).Find(b => b.ExternId == room["Building"].Value<int>());
                    var mRoom = ((List<Room>) building.Rooms).Find(r => r.ExternId == room["Id"].Value<int>());
                    rooms.Add(mRoom);
                }

                var courses = new List<Course>();
                foreach (var course in item["Courses"])
                {
                    var mCourse = ((List<Course>) Courses).Find(c => c.ExternId == course.Value<int>());
                    courses.Add(mCourse);
                }

                // In the API the lessons and evaluation came together so when the type isn't T TP or PL it is considered an evaluation
                if (lessonType == "T" || lessonType == "TP" || lessonType == "PL")
                {
                    var teacher = ((List<User>) Teachers).Find(t => t.ExternId == item["Teacher"].Value<int>());
                    var name = GenerateLessonName(courses);
                    var mLesson = DbManager.Instance.ReturnLessonIfExists(name, startTime, endTime);
                    if (mLesson == null)
                    {
                        Items.Add(CreateLesson(itemId, name, startTime, endTime, rooms, courses, lessonType, teacher));
                    }
                    else
                    {
                        var mItem = CreateLesson(itemId, name, startTime, endTime, rooms, courses, lessonType, teacher,
                            mLesson.Color);
                        Items.Add(mItem);
                        AddAlertsToLesson(mLesson, (TimeTableItems.Lesson) mItem);
                    }
                }
                else
                {
                    var name = GenerateEvaluationName(courses);
                    var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(name, startTime, endTime);
                    if (mEvaluation == null)
                    {
                        Items.Add(CreateEvaluationMoment(itemId, startTime, endTime, rooms, courses, name));
                    }
                    else
                    {
                        var mItem = CreateEvaluationMoment(itemId, startTime, endTime, rooms, courses, name,
                            mEvaluation.Color);
                        Items.Add(mItem);
                        AddAlertsToEvaluation(mEvaluation, (TimeTableItems.EvaluationMoment) mItem);
                    }
                }
            }
        }

        /// <summary>
        ///     Checks to DB to see if there is any events that were not load by DataReader.
        ///     If so they are custom events and it adds them to the items collection
        /// </summary>
        private void AddCustomEvents()
        {
            List<ITimeTableItem> knownEvaluations = null;
            try
            {
                // Creates a list with all the evaluation moments that Item collection already have
                knownEvaluations = ((List<ITimeTableItem>) Items).FindAll(i => i is TimeTableItems.EvaluationMoment);
            }
            catch (ArgumentNullException e)
            {
                // If it returns null it means that there aren't any evaluation moments in the API/File. This is not a problem
                // so we don't need to treat this exception
            }
            foreach (var dbEvMoment in DbManager.Instance.EvaluationMoments())
            {
                if (knownEvaluations != null)
                {
                    var knownEv = knownEvaluations.FirstOrDefault(
                        e =>
                            e.Name == dbEvMoment.Name && e.StartTime == dbEvMoment.StartTime &&
                            e.EndTime == dbEvMoment.EndTime);
                    // If the evaluation is already on the list we don't need to do anything
                    if (knownEv != null)
                        return;
                }
                // if there arent' any evaluations on the collection or the event that we are analyzing isn't on the collection
                // we need to add it
                var rooms = new List<Room>();
                foreach (var room in dbEvMoment.Rooms)
                {
                    var building = ((List<Building>) Buildings).FirstOrDefault(b => b.Name == room.Building.Name);
                    rooms.Add(
                        ((List<Room>) building.Rooms).FirstOrDefault(r => r.Name == room.Name && r.Floor == room.Floor));
                }

                var courses = new List<Course> {Courses.FirstOrDefault(c => c.Name == dbEvMoment.Course)};
                var mItem = CreateEvaluationMoment(dbEvMoment.StartTime, dbEvMoment.EndTime, rooms, courses,
                    dbEvMoment.Name, dbEvMoment.Color, dbEvMoment.Description, true);
                Items.Add(mItem);
                AddAlertsToEvaluation(dbEvMoment, (TimeTableItems.EvaluationMoment) mItem);
            }
        }

        #region Add Events

        /// <summary>
        ///     Add existing Alerts to lesson
        /// </summary>
        /// <param name="dbLesson"></param>
        /// <param name="lesson"></param>
        private void AddAlertsToLesson(Lesson dbLesson, TimeTableItems.Lesson lesson)
        {
            foreach (var alert in dbLesson.Alerts)
            {
                var aAlert = new Alert(alert.ID, alert.AlertTime, lesson);
            }
        }

        /// <summary>
        ///     Add existing Alerts to office hours
        /// </summary>
        /// <param name="dbOfficeHour"></param>
        /// <param name="officeHours"></param>
        private void AddAlertsToOfficeHour(OfficeHour dbOfficeHour, OfficeHours officeHours)
        {
            foreach (var alert in dbOfficeHour.Alerts)
            {
                var aAlert = new Alert(alert.ID, alert.AlertTime, officeHours);
            }
        }

        /// <summary>
        ///     Add existing Alerts to evaluation
        /// </summary>
        /// <param name="dbEvaluation"></param>
        /// <param name="evaluation"></param>
        private void AddAlertsToEvaluation(EvaluationMoment dbEvaluation, TimeTableItems.EvaluationMoment evaluation)
        {
            foreach (var alert in dbEvaluation.Alerts)
            {
                var aAlert = new Alert(alert.ID, alert.AlertTime, evaluation);
            }
        }

        #endregion

        // The methods above are used to generate the names of the items

        #region Generate Item Names

        private string GenerateLessonName(ICollection<Course> courses)
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
            return name;
        }

        private string GenerateEvaluationName(ICollection<Course> courses)
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
            return name;
        }

        private string GenerateOfficeHourName(string teacherName)
        {
            return "Horário de Atendimento de " + teacherName;
        }

        #endregion

        #region Calls Factory.

        private ITimeTableItem CreateEvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms,
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

        #endregion
    }
}