using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Ajax.Utilities;
using Microsoft.Graph;
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
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }

        }

        public override void hook(TimeTableManager manager)
        {
            //Convert to icolliction
            ICollection<Room> rooms = new List<Room>();
            rooms.Add(Request.QueryString["room"]);



            ICollection<Course> course = new List<Course>();
            course.Add(Request.QueryString["course"]);

            ITimeTableItem newEvent = new EvaluationMoment(
                Convert.ToDateTime(Request.QueryString["beginsAt"]),
                Convert.ToDateTime(Request.QueryString["endsAt"]),
                rooms,
                course,
                Request.QueryString["title"], Request.QueryString["description"]);



           
           


            //Request.QueryString["start"] = beginsAt;
            //Request.QueryString["end"] = endsAt;
        }
            //Add Event



                //Return The List Of Events

            }



    }
}