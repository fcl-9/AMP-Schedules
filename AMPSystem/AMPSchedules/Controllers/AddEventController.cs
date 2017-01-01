using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Quartz.Xml;
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
            Validate();
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
            if (rooms.Count == 0 || mRooms.Count == 0)
                return RedirectToAction("Index", "Error",
                    new { message = "There is a problem with the selected room." });

            //Get The course
            ICollection<Course> courses = new List<Course>();
            foreach (var course in manager.Repository.Courses)
                if (course.Name == Request.QueryString["course"])
                    courses.Add(course);

            if (courses.Count == 0)
                return RedirectToAction("Index", "Error",
                    new { message = "There is a problem with the selected course." });

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

        private void Validate()
        {
            if (Request.QueryString["room"] == null)
            {
                throw new ValidationException("You need to select a room.");
            }
            if (Request.QueryString["course"] == null)
            {
                throw new ValidationException("You need to select a course"); 
            }
            if (Request.QueryString["beginsAt"] == null)
            {
                throw new ValidationException("You need to select a date and time to the start of the event");
            }
            if (Request.QueryString["endsAt"] == null)
            {
                throw new ValidationException("You need to select a date and time to the end of the event");
            }
            if (Request.QueryString["title"] == null)
            {
                throw new ValidationException("You need to give a name to the event");
            }
        }
    }
}