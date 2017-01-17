using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AMPSystem.Models;

namespace AMPSystem.DAL
{
    /// <summary>
    ///     Class that permit the connection and manipulation to the Database.
    /// </summary>
    public class DbManager
    {
        private readonly AmpDbContext db = new AmpDbContext();

        /// <summary>
        ///     Return a DB Lesson if exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Lesson ReturnLessonIfExists(string name, DateTime startTime, DateTime endTime, User user)
        {
            if (user != null)
            {
                return
                db.Lessons.Include("Room.Building")
                    .FirstOrDefault(l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime && l.UserID == user.ID);
            }
            return null;
        }

        /// <summary>
        ///     Return a DB EvaluationMoment if exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public EvaluationMoment ReturnEvaluationMomentIfExists(string name, DateTime startTime,
            DateTime endTime, User user)
        {
            if (user != null)
            {
                return
                db.EvaluationMoments.Include("Rooms.Building").FirstOrDefault(
                    l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime && l.UserID == user.ID);
            }
            return null;
        }

        /// <summary>
        ///     Return a DB OfficeHour if exists
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public OfficeHour ReturnOfficeHourIfExists(string name, DateTime startTime, DateTime endTime, User user)
        {
            if (user != null)
            {
                return
                    db.OfficeHours.Include("Room.Building")
                        .FirstOrDefault(l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime && l.UserID == user.ID);
            }
            return null;
        }

        /// <summary>
        ///     Creates a new Lesson to the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Lesson CreateLesson(string name, Room room, User user, string color, DateTime startTime, DateTime endTime)
        {
            var mLesson = new Lesson
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Room = room,
                User = user,
                Alerts = new List<Alert>()
            };
            db.Lessons.Add(mLesson);
            return mLesson;
        }

        /// <summary>
        ///     Creates a new OfficeHour to the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public OfficeHour CreateOfficeHour(string name, Room room, User user, string color, DateTime startTime,
            DateTime endTime)
        {
            var mOfficeHour = new OfficeHour
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Room = room,
                User = user,
                Alerts = new List<Alert>()
            };
            db.OfficeHours.Add(mOfficeHour);
            return mOfficeHour;
        }

        /// <summary>
        ///     Creates an Evaluation Moment to the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rooms"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public EvaluationMoment CreateEvaluationMoment(string name, ICollection<Room> rooms, User user, string color,
            DateTime startTime,
            DateTime endTime, string description, string course, string reminder)
        {
            var mEvaluationMoment = new EvaluationMoment
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Rooms = rooms,
                User = user,
                Description = description,
                Course = course,
                Alerts = new List<Alert>(),
                Reminder = reminder
            };
            db.EvaluationMoments.Add(mEvaluationMoment);
            return mEvaluationMoment;
        }

        /// <summary>
        ///     Creates a new building for the DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Building CreateBuilding(string name)
        {
            var mBuilding = new Building
            {
                Name = name
            };
            db.Buildings.Add(mBuilding);
            return mBuilding;
        }

        /// <summary>
        ///     Creates a new room for the DB
        /// </summary>
        /// <param name="building"></param>
        /// <param name="floor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Room CreateRoom(Building building, int floor, string name)
        {
            var mRoom = new Room
            {
                Building = building,
                Floor = floor,
                Name = name
            };
            db.Rooms.Add(mRoom);
            return mRoom;
        }

        /// <summary>
        ///     Creates a new user for the DB
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User CreateUser(string email)
        {
            var mUser = new User
            {
                Email = email
            };
            db.Users.Add(mUser);
            return mUser;
        }

        /// <summary>
        ///     Returns a DB Room reference if it is already on the DB, null otherwise
        /// </summary>
        /// <param name="buildingName"></param>
        /// <param name="roomName"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public Room ReturnRoomIfExists(string buildingName, string roomName, int floor)
        {
            // The include allow us to acces the building outside the manager
            return db.Rooms.Include("Building").FirstOrDefault(
                r =>
                    r.Building.Name == buildingName && r.Name == roomName &&
                    r.Floor == floor);
        }

        /// <summary>
        ///     Returns a DB building reference if it is already on the DB, null otherwise
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Building ReturnBuildingIfExists(string name)
        {
            // The include allow us to acces the rooms of the building outside the manager
            return db.Buildings.Include("Rooms").FirstOrDefault(b => b.Name == name);
        }

        /// <summary>
        ///     Returns a DB user reference if it is already on the DB, null otherwise
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User ReturnUserIfExists(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        /// <summary>
        ///     Changes the color of a lesson in the database
        /// </summary>
        /// <param name="lesson"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Lesson SaveLessonColorChange(Lesson lesson, string color)
        {
            lesson.Color = color;
            db.Entry(lesson).State = EntityState.Modified;
            return lesson;
        }

        /// <summary>
        ///     Changes the color of an evaluation in the database
        /// </summary>
        /// <param name="evaluation"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public EvaluationMoment SaveEvaluationColorChange(EvaluationMoment evaluation, string color)
        {
            evaluation.Color = color;
            db.Entry(evaluation).State = EntityState.Modified;
            return evaluation;
        }

        /// <summary>
        ///     Changes the color of an office hour in the database
        /// </summary>
        /// <param name="officeHour"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public OfficeHour SaveOfficeHourColorChange(OfficeHour officeHour, string color)
        {
            officeHour.Color = color;
            db.Entry(officeHour).State = EntityState.Modified;
            return officeHour;
        }

        /// <summary>
        ///     Changes the reminder of a lesson in the database
        /// </summary>
        /// <param name="lesson"></param>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public Lesson SaveLessonReminderChange(Lesson lesson, string reminder)
        {
            lesson.Reminder = reminder;
            db.Entry(lesson).State = EntityState.Modified;
            return lesson;
        }

        /// <summary>
        ///     Changes the reminder of an evaluation in the database
        /// </summary>
        /// <param name="evaluation"></param>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public EvaluationMoment SaveEvaluationReminderChange(EvaluationMoment evaluation, string reminder)
        {
            evaluation.Reminder = reminder;
            db.Entry(evaluation).State = EntityState.Modified;
            return evaluation;
        }

        /// <summary>
        ///     Changes the reminder of an office hour in the database
        /// </summary>
        /// <param name="officeHour"></param>
        /// <param name="reminder"></param>
        /// <returns></returns>
        public OfficeHour SaveOfficeHourReminderChange(OfficeHour officeHour, string reminder)
        {
            officeHour.Reminder = reminder;
            db.Entry(officeHour).State = EntityState.Modified;
            return officeHour;
        }

        /// <summary>
        ///     Returns an existing user or creates one if it doesn't exist yet ond the DB
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User CreateUserIfNotExists(string email)
        {
            var user = ReturnUserIfExists(email);
            return user ?? CreateUser(email);
        }

        /// <summary>
        ///     Returns an existing builidng or creates one if it doesn't exist yet ond the DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Building CreateBuildingIfNotExists(string name)
        {
            var building = ReturnBuildingIfExists(name);
            return building ?? CreateBuilding(name);
        }

        /// <summary>
        ///     Returns an existing room or creates one if it doesn«t exist yet ond the DB
        /// </summary>
        /// <param name="building"></param>
        /// <param name="floor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Room CreateRoomIfNotExists(Building building, int floor, string name)
        {
            var room = ReturnRoomIfExists(building.Name, name, floor);
            return room ?? CreateRoom(building, floor, name);
        }

        /// <summary>
        ///     Returns an existing lesson or creates one if it doesn«t exist yet ond the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Lesson CreateLessonIfNotExists(string name, Room room, User user, string color, DateTime startTime,
            DateTime endTime)
        {
            var lesson = ReturnLessonIfExists(name, startTime, endTime,user);
            return lesson ?? CreateLesson(name, room, user, color, startTime, endTime);
        }

        /// <summary>
        ///     Returns an existing office hour or creates one if it doesn«t exist yet ond the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public OfficeHour CreateOfficeHourIfNotExists(string name, Room room, User user, string color,
            DateTime startTime, DateTime endTime)
        {
            var officeHour = ReturnOfficeHourIfExists(name, startTime, endTime, user);
            return officeHour ?? CreateOfficeHour(name, room, user, color, startTime, endTime);
        }

        /// <summary>
        ///     Returns an existing evaluation or creates one if it doesn«t exist yet ond the DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rooms"></param>
        /// <param name="user"></param>
        /// <param name="color"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public EvaluationMoment CreateEvaluationMomentIfNotExists(string name, ICollection<Room> rooms, User user,
            string color, DateTime startTime, DateTime endTime, string description, string course, string reminder)
        {
            var evMoment = ReturnEvaluationMomentIfExists(name, startTime, endTime, user);
            return evMoment ?? CreateEvaluationMoment(name, rooms, user, color, startTime, endTime, description, course, reminder);
        }

        /// <summary>
        ///     Saves the changes on the DB
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        /// <summary>
        ///     Returns all the evaluation moments from user in the DB
        /// </summary>
        /// <returns></returns>
        public ICollection<EvaluationMoment> EvaluationMoments(User user)
        {
            return db.EvaluationMoments.Where(e => e.UserID == user.ID).Include("Rooms.Building").ToList();
        }

        /// <summary>
        ///     Removes a give evaluation moment from the DB
        /// </summary>
        /// <param name="evaluationMoment"></param>
        public void RemoveEvent(EvaluationMoment evaluationMoment)
        {
            db.Entry(evaluationMoment).State = EntityState.Deleted;

            db.SaveChanges();
        }

        /// <summary>
        ///     Add alert to a lesson
        /// </summary>
        /// <param name="alertTime"></param>
        /// <param name="lesson"></param>
        public Alert AddAlertToLesson(DateTime alertTime, Lesson lesson)
        {
            var mAlert = new Alert
            {
                AlertTime = alertTime,
                Lesson = lesson
            };
            db.Alerts.Add(mAlert);
            return mAlert;
        }

        /// <summary>
        ///     Add alerto to Evaluation
        /// </summary>
        /// <param name="alertTime"></param>
        /// <param name="evaluationMoment"></param>
        public Alert AddAlertToEvaluation(DateTime alertTime, EvaluationMoment evaluationMoment)
        {
            var mAlert = new Alert
            {
                AlertTime = alertTime,
                EvaluationMoment = evaluationMoment
            };
            db.Alerts.Add(mAlert);
            return mAlert;
        }

        /// <summary>
        ///     Add alert to Office hour
        /// </summary>
        /// <param name="alertTime"></param>
        /// <param name="officeHour"></param>
        public Alert AddAlertToOfficeH(DateTime alertTime, OfficeHour officeHour)
        {
            var mAlert = new Alert
            {
                AlertTime = alertTime,
                OfficeHour = officeHour
            };
            db.Alerts.Add(mAlert);
            return mAlert;
        }

        /// <summary>
        ///     Removes an alert
        /// </summary>
        /// <param name="alert"></param>
        public void RemoveAlert(Alert alert)
        {
            db.Entry(alert).State = EntityState.Deleted;

            db.SaveChanges();
        }

        /// <summary>
        ///     Returns an alert with a given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Alert ReturnAlert(int id)
        {
            return db.Alerts.Find(id);
        }

        #region Singleton

        private static DbManager _instance;

        private DbManager()
        {
        }

        public static DbManager Instance => _instance ?? (_instance = new DbManager());

        #endregion
    }
}