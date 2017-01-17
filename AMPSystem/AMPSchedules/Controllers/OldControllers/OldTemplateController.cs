using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace AMPSchedules.Controllers
{
    [Authorize]
    public abstract class OldTemplateController : Controller
    {
        private static readonly object _lockobject = new object();
        public User CurrentUser { get; private set; }

        public virtual ActionResult Hook()
        {
            var parsedItems = ParseData(TimeTableManager.Instance);
            return
                Content(
                    JsonConvert.SerializeObject(parsedItems.ToArray(),
                        new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    "application/json");
        }

        public async Task<ActionResult> TemplateMethod()
        {
            LoadData();
            return Hook();
        }

        private void LoadData()
        {
            var mail = new MailAddress(ClaimsPrincipal.Current.FindFirst("preferred_username")?.Value);

            var user = mail.User;
            lock (_lockobject)
            {
                if (!Repository.Instance.DataLoaded)
                {
                    IDataReader dataReader = new FileData();
                    Repository.Instance.DataReader = dataReader;
                    Repository.Instance.CleanRepository();
                    Repository.Instance.LoadData(mail);
                }
            }

            var roles = new List<string>();

            var domain = mail.Host;
            if (domain == "student.uma.pt")
            {
                roles.Add("Student");
            }
            else
            {
                roles.Add("Teacher");
            }

            CurrentUser = Factory.Instance.CreateUser(user, mail.Address, roles, Repository.Instance.UserCourses);
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager.Instance.CreateTimeTable(startDateTime, endDateTime);
        }

        private IList<CalendarItem> ParseData(TimeTableManager manager)
        {
            IList<CalendarItem> parsedItems = new List<CalendarItem>();

            foreach (var item in manager.TimeTable.ItemList)
            {
                CalendarItem adapter = new ItemAdapter(item);
                parsedItems.Add(adapter);
            }
            //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
            //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
            return parsedItems;
        }
    }
}