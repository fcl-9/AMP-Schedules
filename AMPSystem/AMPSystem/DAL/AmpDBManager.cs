using System;
using System.Collections.Generic;
using System.Linq;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.Interfaces;

namespace AMPSystem.DAL
{
    public class AmpDbManager
    {
        #region Singleton
        private static AmpDbManager _instance;

        private AmpDbManager() { }

        public static AmpDbManager Instance => _instance ?? (_instance = new AmpDbManager());
        #endregion

        /// <summary>
        /// Method used to check if a User exist in the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UserExists(int id)
        {
            using (var db = new AmpContext())
            {
                return db.Users.Any(o => o.UserID == id);
            }
        }

        /// <summary>
        /// Method used to check if a user has alerts.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasAlerts(int id)
        {
            using (var db = new AmpContext())
            {
                return db.Alerts.Any(o => o.AlertID == id);
            }
        }

        /// <summary>
        /// Method used to check if a User exist in the DB.
        /// TODO type? Replace id for email?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasTimeTableItems(string type, int id)
        {
            using (var db = new AmpContext())
            {
                switch (type)
                {
                    case "Lesson":
                        return db.Lessons.Any(o => o.ID == id);
                    case "EvaluationMoment":
                        return db.EvalMoments.Any(o => o.ID == id);
                    case "OfficeHours":
                        return db.OfficeHours.Any(o => o.ID == id);
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITimeTableItem ReturnTableData(string type, int id)
        {
            using (var db = new AmpContext())
            {
                switch (type)
                {
                    case "Lesson":
                        var lessonTable = db.Lessons.SqlQuery("SELECT * FROM dbo.Lesson").ToList();
                        foreach (var raw in lessonTable)
                        {
                            if (raw.ID == id) { return raw; }
                        }
                        return null;
                    case "EvaluationMoment":
                        var evalTable = db.EvalMoments.SqlQuery("SELECT * FROM dbo.EvaluationMoment").ToList();
                        foreach (var raw in evalTable)
                        {
                            if (raw.ID == id) { return raw; }
                        }
                        return null;
                    case "OfficeHours":
                        var officeTable = db.OfficeHours.SqlQuery("SELECT * FROM dbo.OfficeHours").ToList();
                        foreach (var raw in officeTable)
                        {
                            if (raw.ID == id) { return raw; }
                        }
                        return null;
                    default:
                        return null;
                }
            }
        }

        public void AddAlert(params object[] parameters)
        {
            using (var db = new AmpContext())
            {
                if (db.Alerts.Any(o => o.AlertID == (int) parameters[0])) return;

                var alert = new Alert
                {
                    AlertID = (int) parameters[0],
                    Item = (ITimeTableItem) parameters[1],
                    Time = (TimeSpan) parameters[2]
                };

                db.Alerts.Add(alert); //or db.Alerts.AddOrUpdate(alert);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// TODO SEE THIS. Remain thing to do!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        public void AddTimeTableItem(string type, params object[] parameters)
        {
            using (var db = new AmpContext())
            {
                if (type == "Lesson")
                {
                    if (db.Lessons.Any(o => o.ID == (int)parameters[0])) return;

                    var lesson = new Lesson
                    {
                        ID = (int)parameters[0],
                        StartTime = (DateTime) parameters[1],
                        EndTime = (DateTime) parameters[2],
                        Rooms = (IList<Room>) parameters[3],
                        Courses = (IList<Course>)parameters[4],
                        Type = (string) parameters[5],
                        Name = (string) parameters[6],
                        Description = (string) parameters[7]
                    };

                    db.Lessons.Add(lesson);
                    db.SaveChanges();
                }
                else if (type == "EvaluationMoment")
                {
                    if (db.EvalMoments.Any(o => o.ID == (int)parameters[0])) return;

                    var evalMoments = new EvaluationMoment()
                    {
                        ID = (int)parameters[0],
                        StartTime = (DateTime)parameters[1],
                        EndTime = (DateTime)parameters[2],
                        Rooms = (IList<Room>)parameters[3],
                        Courses = (IList<Course>)parameters[4],
                        Name = (string)parameters[5],
                        Description = (string)parameters[6],
                        Color = (string)parameters[7]
                    };

                    db.EvalMoments.Add(evalMoments);
                    db.SaveChanges();
                }
                else if (type == "OfficeHours")
                {
                    if (db.OfficeHours.Any(o => o.ID == (int)parameters[0])) return;

                    var officeHours = new OfficeHours()
                    {
                        ID = (int)parameters[0],
                        StartTime = (DateTime)parameters[1],
                        EndTime = (DateTime)parameters[2],
                        Rooms = (IList<Room>)parameters[3],
                        Name = (string)parameters[4],
                        Description = (string)parameters[5],
                        Color = (string)parameters[6]
                    };

                    db.OfficeHours.Add(officeHours);
                    db.SaveChanges();
                }
            }
        }
    }
}