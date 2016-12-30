using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class AddEventController : TemplateController
    {
        // GET: AddEvent
        [HttpGet]
        public async Task<ActionResult> AddEvent()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            //Get the room
            var roomFullName = Request.QueryString["room"];
            var stringSeparators = new string[] { " - " };
            var buildingName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[0];
            var roomName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[1];
            ICollection<Room> rooms = new List<Room>();
            ICollection<AMPSystem.Models.Room> mRooms = new List<AMPSystem.Models.Room>();
            foreach (var building in manager.Repository.Buildings)
            {
                if (building.Name == buildingName)
                {
                    foreach (var room in building.Rooms)
                    {
                        if (room.Name == roomName)
                        {
                            rooms.Add(room);
                            var mBulding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                            mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBulding, room.Floor, room.Name));
                        }
                    }
                }
            }
            
            //Get The course
            ICollection<Course> courses = new List<Course>();
            foreach (var course in manager.Repository.Courses)
            {
                if (course.Name == Request.QueryString["course"])
                {
                    courses.Add(course);
                }
            }
            var startTime = Convert.ToDateTime(Request.QueryString["beginsAt"]);
            var endTime = Convert.ToDateTime(Request.QueryString["endsAt"]);
            var name = Request.QueryString["title"];
            var description = Request.QueryString["description"];
            var editable = true;
            //Get the course
            ITimeTableItem newEvent = new EvaluationMoment(
                startTime,
                endTime,
                rooms,
                courses, 
                name,
                description,
                editable
                );
            newEvent.Editable = true;
            //Add new Event
            manager.AddTimetableItem(newEvent);
            var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
            DbManager.Instance.CreateEvaluationMoment(name, mRooms, mUser, null, startTime, endTime, description,
                courses.First().Name, null);
            DbManager.Instance.SaveChanges();
            return base.Hook(manager);
        }
    }
}