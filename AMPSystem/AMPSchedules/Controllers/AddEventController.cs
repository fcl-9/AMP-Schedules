using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Resources;

namespace AMPSchedules.Controllers
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
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            //Get the room
            var roomFullName = Request.QueryString["room"];
            var stringSeparators = new[] {" - "};
            var buildingName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[0];
            var roomName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[1];
            ICollection<Room> rooms = new List<Room>();
            ICollection<AMPSystem.Models.Room> mRooms = new List<AMPSystem.Models.Room>();
            foreach (var building in manager.Repository.Buildings)
                if (building.Name == buildingName)
                    foreach (var room in building.Rooms)
                        if (room.Name == roomName)
                        {
                            rooms.Add(room);
                            var mBulding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                            mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBulding, room.Floor, room.Name));
                        }

            //Get The course
            ICollection<Course> courses = new List<Course>();
            foreach (var course in manager.Repository.Courses)
                if (course.Name == Request.QueryString["course"])
                    courses.Add(course);
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
                courses.First().Name);
            DbManager.Instance.SaveChanges();
            return base.Hook(manager);
        }
    }
}