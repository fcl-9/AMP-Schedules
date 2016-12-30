using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AMPSystem.Interfaces;
using AMPSystem.Models;

namespace AMPSystem.DAL
{
    public class DbManager
    {
        private AmpDbContext db = new AmpDbContext();

        #region Singleton

        private static DbManager _instance;

        private DbManager()
        {
        }

        public static DbManager Instance => _instance ?? (_instance = new DbManager());

        #endregion

        /// <summary>
        /// Return a DB Lesson if exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Lesson ReturnLessonIfExists(string name, DateTime startTime, DateTime endTime)
        {
            return db.Lessons.Include("Room.Building").FirstOrDefault(l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime);
        }

        /// <summary>
        /// Return a DB EvaluationMoment if exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public EvaluationMoment ReturnEvaluationMomentIfExists(string name, DateTime startTime,
            DateTime endTime)
        {
            return
                db.EvaluationMoments.Include("Rooms.Building").FirstOrDefault(
                    l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime);
        }

        /// <summary>
        /// Return a DB OfficeHour if exists
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public OfficeHour ReturnOfficeHourIfExists(string name, DateTime startTime, DateTime endTime)
        {
            return db.OfficeHours.Include("Room.Building").FirstOrDefault(l => l.Name == name && l.StartTime == startTime && l.EndTime == endTime);
        }

        public Lesson CreateLesson(string name, Room room, User user, string color, DateTime startTime, DateTime endTime)
        {
            var mLesson = new Lesson
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Room = room,
                User = user
            };
            db.Lessons.Add(mLesson);
            return mLesson;
        }

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
                User = user
            };
            db.OfficeHours.Add(mOfficeHour);
            return mOfficeHour;
        }

        public EvaluationMoment CreateEvaluationMoment(string name, ICollection<Room> rooms, User user, string color, DateTime startTime,
            DateTime endTime, string description)
        {
            var mEvaluationMoment = new EvaluationMoment
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Rooms = rooms,
                User = user,
                Description = description
            };
            db.EvaluationMoments.Add(mEvaluationMoment);
            return mEvaluationMoment;
        }

        public Lesson CreateLesson(string name, Room room, User user, string color, DateTime startTime, DateTime endTime, ICollection<Alert> alerts)
        { 
            var mLesson = new Lesson
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Room = room,
                User = user,
                Alerts = alerts
            };
            db.Lessons.Add(mLesson);
            return mLesson;
        }

        public OfficeHour CreateOfficeHour(string name, Room room, User user, string color, DateTime startTime,
            DateTime endTime, ICollection<Alert> alerts)
        {
            var mOfficeHour = new OfficeHour
            {
                Color = color,
                EndTime = endTime,
                StartTime = startTime,
                Name = name,
                Room = room,
                User = user,
                Alerts = alerts
            };
            db.OfficeHours.Add(mOfficeHour);
            return mOfficeHour;
        }

        public EvaluationMoment CreateEvaluationMoment(string name, ICollection<Room> rooms, User user, string color, DateTime startTime,
            DateTime endTime, string description, string course, ICollection<Alert> alerts)
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
                Alerts = alerts
            };
            db.EvaluationMoments.Add(mEvaluationMoment);
            return mEvaluationMoment;
        }

        public Building CreateBuilding(string name)
        {
            var mBuilding = new Building
            {
                Name = name
            };
            db.Buildings.Add(mBuilding);
            return mBuilding;
        }

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

        public User CreateUser(string email)
        {
            var mUser = new User
            {
                Email = email
            };
            db.Users.Add(mUser);
            return mUser;
        }

        public Room ReturnRoomIfExists(string buildingName, string roomName, int floor)
        {
            return db.Rooms.Include("Building").FirstOrDefault(
                r =>
                    r.Building.Name == buildingName && r.Name == roomName &&
                    r.Floor == floor);
        }

        public Building ReturnBuildingIfExists(string name)
        {
            return db.Buildings.Include("Rooms").FirstOrDefault(b => b.Name == name);
        }

        public User ReturnUserIfExists(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        public Lesson SaveLessonColorChange(Lesson lesson, string color)
        {
            lesson.Color = color;
            db.Entry(lesson).State = System.Data.Entity.EntityState.Modified;
            return lesson;
        }

        public EvaluationMoment SaveEvaluationColorChange(EvaluationMoment evaluation, string color)
        {
            evaluation.Color = color;
            db.Entry(evaluation).State = System.Data.Entity.EntityState.Modified;
            return evaluation;
        }

        public OfficeHour SaveOfficeHourColorChange(OfficeHour officeHour, string color)
        {
            officeHour.Color = color;
            db.Entry(officeHour).State = System.Data.Entity.EntityState.Modified;
            return officeHour;
        }

        public User CreateUserIfNotExists(string email)
        {
            var user = ReturnUserIfExists(email);
            return user ?? CreateUser(email);
        }

        public Building CreateBuildingIfNotExists(string name)
        {
            var building = ReturnBuildingIfExists(name);
            return building ?? CreateBuilding(name);
        }

        public Room CreateRoomIfNotExists(Building building, int floor, string name)
        {
            var room = ReturnRoomIfExists(building.Name, name, floor);
            return room ?? CreateRoom(building,floor,name);
        }

        public Lesson CreateLessonIfNotExists(string name, Room room, User user, string color, DateTime startTime, DateTime endTime)
        {
            var lesson = ReturnLessonIfExists(name, startTime, endTime);
            return lesson ?? CreateLesson(name,room,user,color,startTime,endTime);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public ICollection<EvaluationMoment> EvaluationMoments()
        {
            return db.EvaluationMoments.ToList();
        }

        public void RemoveEvent(EvaluationMoment evaluationMoment)
        {
            db.Entry(evaluationMoment).State = System.Data.Entity.EntityState.Deleted;

            db.SaveChanges();
        }

    }

}