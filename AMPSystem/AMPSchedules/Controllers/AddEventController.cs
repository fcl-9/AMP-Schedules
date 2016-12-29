using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
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
            ICollection<Room> rooms = new List<Room>();
            foreach (var building in manager.Repository.Buildings)
            {
                foreach (var room in building.Rooms)
                {
                    if (room.Id == int.Parse(Request.QueryString["room"]))
                    {
                        rooms.Add(room);
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

            //Get the course
            ITimeTableItem newEvent = new EvaluationMoment(
                Convert.ToDateTime(Request.QueryString["beginsAt"]),
                Convert.ToDateTime(Request.QueryString["endsAt"]),
                rooms,
                courses, 
                Request.QueryString["title"],
                Request.QueryString["description"]
                );
            newEvent.Editable = true;
            //Add new Event
            manager.AddTimetableItem(newEvent);
            return base.Hook(manager);
        }
    }
}