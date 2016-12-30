using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;
using Newtonsoft.Json;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public abstract class TemplateController: Controller
    {
        private static object _lockobject = new object();
        public User CurrentUser { get; private set; }

        public virtual ActionResult Hook(TimeTableManager manager)
        {
            var parsedItems = ParseData(manager);
            return Content(JsonConvert.SerializeObject(parsedItems.ToArray(), new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}), "application/json");
        }

        public async Task<ActionResult> TemplateMethod()
        {
            var manager = await LoadData();
            return Hook(manager);
        }

        private async Task<TimeTableManager> LoadData()
        {
            var x = System.Security.Claims.ClaimsPrincipal.Current;
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Commented for testes only!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();
            //var user = await graphService.GetUsername(graphClient);
            
                IDataReader dataReader = new FileData();
                var loadData = new Repository {DataReader = dataReader};
            
            lock (_lockobject)
            {
                loadData.GetCourses();
                loadData.GetRooms();
                loadData.GetTeachers();
                //!!!!!!!!!!!!!!!!!!!!!!! Commented only for tests!!!!!!!!!!!!!!!!!!!!!!!!
                //loadData.GetUserCourses(user);
                //loadData.GetSchedule(user);
                loadData.GetUserCourses("2054313");
                loadData.GetSchedule("2054313");
                loadData.AddCustomEvents();
                var roles = new List<string> {"Student"};
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
                CurrentUser = Factory.Instance.CreateUser("Vítor Baptista", "2054313@student.uma.pt", roles,
                    loadData.UserCourses);
            }
            //Ends.
            var startDateTime = Convert.ToDateTime(Request.QueryString["start"]);
            var endDateTime = Convert.ToDateTime(Request.QueryString["end"]);
            //The manager will start the timetableitem list with the data read from the repo
            var manager = new TimeTableManager(loadData, startDateTime, endDateTime, CurrentUser);
            return manager;
        }

        public IList<CalendarItem> ParseData(TimeTableManager manager)
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
