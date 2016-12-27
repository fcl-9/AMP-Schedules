using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace AMPSystem.Classes
{
    abstract public class TemplateController: Controller
    {
        public abstract void hook(TimeTableManager manager);

        public async Task<ActionResult> TemplateMethod()
        {
            TimeTableManager manager = await LoadData();
            hook(manager);
            IList<CalendarItem> parsedItems = ParseData(manager);
            return Content(JsonConvert.SerializeObject(parsedItems.ToArray()), "application/json");
        }

        public async Task<TimeTableManager> LoadData()
        {
            //var x = User;
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Commented for testes only!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();
            //var user = await graphService.GetUsername(graphClient);
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses();
            loadData.GetRooms();
            //!!!!!!!!!!!!!!!!!!!!!!! Commented only for tests!!!!!!!!!!!!!!!!!!!!!!!!
            //loadData.GetUserCourses(user);
            //loadData.GetSchedule(user);
            loadData.GetUserCourses("2054313");
            loadData.GetSchedule("2054313");
            loadData.GetTeachers();
            var roles = new List<string>();
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Comented only for tests!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            /*var mail = new MailAddress(await graphService.GetMyEmailAddress(graphClient));
            var domain = mail.Host;
            if (domain == "student.uma.pt")
            {
                roles.Add("Student");
            }
            else
            {
                roles.Add("Teacher");
            }
            Factory.Instance.CreateUser(await graphService.GetUserName(graphClient),
                await graphService.GetMyEmailAddress(graphClient), roles, loadData.UserCourses);*/
            roles.Add("Student");
            Factory.Instance.CreateUser("Vítor Baptista", "2054313@student.uma.pt", roles, loadData.UserCourses);
            //acaba
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager Manager = new TimeTableManager(loadData, startDateTime, endDateTime);
            return Manager;
        }

        

        public IList<CalendarItem> ParseData(TimeTableManager Manager)
        {
            IList<CalendarItem> parsedItems = new List<CalendarItem>();

            foreach (var item in Manager.TimeTable.ItemList)
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
