using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;
using Quartz.Xml;

namespace AMPSchedules.Controllers
{
    public class EventsController : BaseController
    {
        public ActionResult Add()
        {
            var facade = PrepareAndGetFacade();

            Validate();

            var roomFullName = Request.QueryString["room"];
            var stringSeparators = new[] { " - " };
            var buildingName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[0];
            var roomName = roomFullName.Split(stringSeparators, StringSplitOptions.None)[1];

            var courseName = Request.QueryString["course"];

            var startTime = Convert.ToDateTime(Request.QueryString["beginsAt"]);
            var endTime = Convert.ToDateTime(Request.QueryString["endsAt"]);
            var name = Request.QueryString["title"];
            var description = Request.QueryString["description"];
            var reminder = Request.QueryString["reminder"];

            try
            {
                return
                    Content(
                        JsonConvert.SerializeObject(
                            facade.AddEvent(buildingName, roomName, courseName, startTime, endTime, name, description,
                                reminder),
                            new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                        "application/json");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error",
                    new { message =  e.Message });
            }

        }

        public ActionResult Remove()
        {
            var facade = PrepareAndGetFacade();

            return Content(JsonConvert.SerializeObject(facade.RemoveEvent(Request.QueryString["name"], Convert.ToDateTime(Request.QueryString["startEvent"])),
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    "application/json");
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