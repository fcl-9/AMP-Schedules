/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Resources;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using AMPSystem.Classes;

using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json;


namespace AMPSchedules.Controllers
{
    //Commented for tests only!!!!!!!!!!!!!![Authorize]
    public class HomeController : Controller
    {
        GraphService graphService = new GraphService();

        public ActionResult Index()
        {
            return View("Graph");
        }

        private async Task<Repository> getData()
        {
            /* !!!!!!!!!!!!!!!!!!!!!!! Commented only for tests!!!!!!!!!!!!!!!!!!!!!!!!
            // Initialize the GraphServiceClient.
            GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

            // Get the current user's email address. 
            var email = await graphService.GetMyEmailAddress(graphClient);
            var mail = new MailAddress(email);
            var user = mail.User;*/
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses(Server.MapPath(@"~/App_Data/Cadeiras"));
            loadData.GetRooms(Server.MapPath(@"~/App_Data/Salas"));
            //!!!!!!!!!!!!!!!!!!!!!!! Commented only for tests!!!!!!!!!!!!!!!!!!!!!!!!
            //loadData.GetUserCourses(Server.MapPath(@"~/App_Data/Course/" + user));
            //loadData.GetSchedule(Server.MapPath(@"~/App_Data/Schedule/" + user));
            loadData.GetUserCourses(Server.MapPath(@"~/App_Data/Course/2054313"));
            loadData.GetSchedule(Server.MapPath(@"~/App_Data/Schedule/2054313"));
            loadData.GetTeachers(Server.MapPath(@"~/App_Data/Teacher"));
            return loadData;
        }

        // API Controller
        public async Task<ActionResult> CalendarDefaultData()
        {
            try
            {
                var loadData = await getData();
                //Default interval of the view
                var date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                Timetable timetable = new Timetable(firstDayOfMonth, lastDayOfMonth);
                //The manager will start the timetableitem list with the data read from the repo
                TimeTableManager Manager = new TimeTableManager(timetable, loadData);

                IList<CalendarItem> parsedItems = new List<CalendarItem>();

                foreach (var item in Manager.TimeTable.ItemList)
                {
                    CalendarItem adapter = new ItemAdapter(item);
                    parsedItems.Add(adapter);
                }
                //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
                //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
                return Content(JsonConvert.SerializeObject(parsedItems.ToArray()), "application/json");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        //Handles every request that was made by a user to filter it's activities
        [HttpGet]
        public async Task<ActionResult> AddFilter()
        {
            try
            {
                var loadData = await getData();
                //Default interval of the view
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                Timetable timetable = new Timetable(firstDayOfMonth, lastDayOfMonth);

                //The manager will start the timetableitem list with the data read from the repo
                TimeTableManager Manager = new TimeTableManager(timetable, loadData);

                //TODO: Is this a hook ??? --> Templte Method ??
                AndCompositeFilter Filters = new AndCompositeFilter(Manager);
                foreach (var filter in Request.QueryString)
                {
                    if (Request.QueryString[(string)filter] == "ClassName")
                    {
                        IFilter nameFilter = new Name((string)filter, Manager);
                        Filters.Add(nameFilter);
                    }
                    else if (Request.QueryString[(string)filter] == "Type")
                    {
                        IFilter typeFilter = new TypeF((string)filter, Manager);
                        Filters.Add(typeFilter);
                    }
                }

                Filters.ApplyFilter();
                //TODO: HOOK END

                IList<CalendarItem> parsedItems = new List<CalendarItem>();

                foreach (var item in Manager.TimeTable.ItemList)
                {
                    CalendarItem adapter = new ItemAdapter(item);
                    parsedItems.Add(adapter);
                }

                return Content(JsonConvert.SerializeObject(parsedItems.ToArray()), "application/json");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }






        //Change Color of an Event Group Based on the Selected Event
        [HttpGet]
        public async Task<ActionResult> EventColor()
        {
            try
            {
                var loadData = await getData();
                //Default interval of the view
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                Timetable timetable = new Timetable(firstDayOfMonth, lastDayOfMonth);

                //The manager will start the timetableitem list with the data read from the repo
                TimeTableManager Manager = new TimeTableManager(timetable, loadData);

                //Read The Color that was sent
                string color = null;
                string itemName = null;
                foreach (var eventName in Request.QueryString)
                {
                    itemName = (string)eventName;
                    color = Request.QueryString[itemName];
                }

                //Change the color on the items 
                foreach (var item in Manager.TimeTable.ItemList)
                {
                    if (item.Name == itemName)
                    {
                        if (color != null)
                        {

                            item.Color = color;
                            //Debug.Write(item.Color);
                        }
                        else
                        {
                            Debug.Write("No Color was Defined");
                            break;
                        }
                    }
                }




                IList<CalendarItem> parsedItems = new List<CalendarItem>();
                foreach (var item in Manager.TimeTable.ItemList) { 
                    CalendarItem adapter = new ItemAdapter(item);
                    parsedItems.Add(adapter);
                }

                return Content(JsonConvert.SerializeObject(parsedItems.ToArray()), "application/json");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        // Get the current user's email address from their profile.
        public async Task<ActionResult> GetMyEmailAddress()
        {
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the current user's email address. 
                ViewBag.Email = await graphService.GetMyEmailAddress(graphClient);
                return View("Graph");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }






        // Send mail on behalf of the current user.
        public async Task<ActionResult> SendEmail()
        {
            if (string.IsNullOrEmpty(Request.Form["email-address"]))
            {
                ViewBag.Message = Resource.Graph_SendMail_Message_GetEmailFirst;
                return View("Graph");
            }

            // Build the email message.
            Message message = graphService.BuildEmailMessage(Request.Form["recipients"], Request.Form["subject"]);
            try
            {

                // Initialize the GraphServiceClient.
                GraphServiceClient graphClient = SDKHelper.GetAuthenticatedClient();

                // Send the email.
                await graphService.SendEmail(graphClient, message);

                // Reset the current user's email address and the status to display when the page reloads.
                ViewBag.Email = Request.Form["email-address"];
                ViewBag.Message = Resource.Graph_SendMail_Success_Result;
                return View("Graph");
            }
            catch (ServiceException se)
            {
                if (se.Error.Code == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

    }
}
