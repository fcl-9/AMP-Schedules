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
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool HasTimeTableItems(string type, string control, int id)
        {
            using (var db = new AmpContext())
            {
                switch (type)
                {
                    case "Lesson":
                        return control == "ID" ? db.Lessons.Any(o => o.ID == id) : db.Lessons.Any(o => o.ExternId == id);
                    case "EvaluationMoment":
                        return control == "ID" ? db.EvalMoments.Any(o => o.ID == id) : db.EvalMoments.Any(o => o.ExternId == id);
                    case "OfficeHours":
                        return control == "ID" ? db.OfficeHours.Any(o => o.ID == id) : db.OfficeHours.Any(o => o.ExternId == id);
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns the ITimeTableItem
        /// </summary>
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static ITimeTableItem ReturnTableData(string type, string control, int Id)
        {
            using (var db = new AmpContext())
            {
                switch (type)
                {
                    case "Lesson":
                        var lessonTable = db.Lessons.SqlQuery("SELECT * FROM dbo.Lesson").ToList();
                        foreach (var row in lessonTable)
                        {
                            switch (control)
                            {
                                case "ID":
                                    if (row.ID == Id) { return row; }
                                    break;
                                case "ExternID":
                                    if (row.ExternId == Id) { return row; }
                                    break;
                            }
                        }
                        return null;
                    case "EvaluationMoment":
                        var evalTable = db.EvalMoments.SqlQuery("SELECT * FROM dbo.EvaluationMoment").ToList();
                        foreach (var row in evalTable)
                        {
                            switch (control)
                            {
                                case "ID":
                                    if (row.ID == Id) { return row; }
                                    break;
                                case "ExternID":
                                    if (row.ExternId == Id) { return row; }
                                    break;
                            }
                        }
                        return null;
                    case "OfficeHours":
                        var officeTable = db.OfficeHours.SqlQuery("SELECT * FROM dbo.OfficeHours").ToList();
                        foreach (var row in officeTable)
                        {
                            switch (control)
                            {
                                case "ID":
                                    if (row.ID == Id) { return row; }
                                    break;
                                case "ExternID":
                                    if (row.ExternId == Id) { return row; }
                                    break;
                            }
                        }
                        return null;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Return a item if exists.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITimeTableItem ReturnItemIfExists(string type, string control, int id)
        {
            return HasTimeTableItems(type, control, id) ? ReturnTableData(type, control, id) : null;
        }

        public void UpdateTimeTableItem(ITimeTableItem item)
        {
            using (var db = new AmpContext())
            {
                if (item is Lesson)
                {
                    var original = db.Lessons.Find(item.ID);
                    db.Entry(original).CurrentValues.SetValues(item);
                    db.SaveChanges();
                }
                else if (item is EvaluationMoment)
                {
                    var original = db.EvalMoments.Find(item.ID);
                    db.Entry(original).CurrentValues.SetValues(item);
                    db.SaveChanges();
                }
                else if (item is OfficeHours)
                {
                    var original = db.OfficeHours.Find(item.ID);
                    db.Entry(original).CurrentValues.SetValues(item);
                    db.SaveChanges();
                }
            }
        }

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
        /// Add the current user into database.
        /// </summary>
        /// <param name="currentUser"></param>
        public void AddUser(User currentUser)
        {
            using (var db = new AmpContext())
            {
                if (db.Users.Any(o => o.UserID == currentUser.UserID)) return;
                db.Users.Add(currentUser); //or db.Alerts.AddOrUpdate(alert);
                db.SaveChanges();
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
        /// Add a alert into database.
        /// </summary>
        /// <param name="alert"></param>
        public void AddAlert(Alert alert)
        {
            using (var db = new AmpContext())
            {
                if (db.Alerts.Any(o => o.AlertID == alert.AlertID)) return;
                db.Alerts.Add(alert); //or db.Alerts.AddOrUpdate(alert);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Remove a alert from database.
        /// </summary>
        /// <param name="alert"></param>
        public void RemoveAlert(Alert alert)
        {
            using (var db = new AmpContext())
            {
                if (db.Alerts.Any(o => o.AlertID == alert.AlertID)) return;
                db.Alerts.Remove(alert); //or db.Alerts.AddOrUpdate(alert);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add a time table item into Database.
        /// </summary>
        /// <param name="item"></param>
        public void AddTimeTableItem(ITimeTableItem item)
        {
            using (var db = new AmpContext())
            {
                if (item is Lesson)
                {
                    if (db.Lessons.Any(o => o.ID == item.ID)) return;
                    db.Lessons.Add((Lesson)item);
                    db.SaveChanges();
                }
                else if(item is EvaluationMoment)
                {
                    if (db.EvalMoments.Any(o => o.ID == item.ID)) return;
                    db.EvalMoments.Add((EvaluationMoment)item);
                    db.SaveChanges();
                }
                else if (item is OfficeHours)
                {
                    if (db.OfficeHours.Any(o => o.ID == item.ID)) return;
                    db.OfficeHours.Add((OfficeHours)item);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Remove a time table item from Database.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveTimeTableItem(ITimeTableItem item)
        {
            using (var db = new AmpContext())
            {
                if (item is Lesson)
                {
                    if (db.Lessons.Any(o => o.ID == item.ID)) return;
                    db.Lessons.Remove((Lesson)item);
                    db.SaveChanges();
                }
                else if (item is EvaluationMoment)
                {
                    if (db.EvalMoments.Any(o => o.ID == item.ID)) return;
                    db.EvalMoments.Remove((EvaluationMoment)item);
                    db.SaveChanges();
                }
                else if (item is OfficeHours)
                {
                    if (db.OfficeHours.Any(o => o.ID == item.ID)) return;
                    db.OfficeHours.Remove((OfficeHours)item);
                    db.SaveChanges();
                }
            }
        }
    }
}