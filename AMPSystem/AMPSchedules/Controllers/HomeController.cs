﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
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

    public class HomeController : Controller
    {
        GraphService graphService = new GraphService();

        public ActionResult Index()
        {
            return View("Graph");
        }

        // Controller actions
        public ActionResult About()
        {
            DataReader dataReader = new FileData();
            Repository loadData = new Repository();
            loadData.DataReader = dataReader;
            loadData.GetCourses(Server.MapPath(@"~/App_Data/Cadeiras"));
            loadData.GetRooms(Server.MapPath(@"~/App_Data/Salas"));
            loadData.GetSchedule(Server.MapPath(@"~/App_Data/Dados"));
            loadData.GetTeachers(Server.MapPath(@"~/App_Data/Teacher"));
            //Default interval of the view
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            Timetable timetable = new Timetable(firstDayOfMonth, lastDayOfMonth);
            //The manager will start the timetableitem list with the data read from the repo
            TimeTableManager Manager = new TimeTableManager(timetable,loadData);

            IList<CalendarItem> parsedItems = new List<CalendarItem>(); 

            foreach (var item in Manager.TimeTable.ItemList)
            {
                CalendarItem adapter = new ItemAdapter(item);
                parsedItems.Add(adapter); 
            }
            //This flag , "JsonRequestBehavior.AllowGet" removes protection from gets 
            //return Json( TimeTableItemsList , JsonRequestBehavior.AllowGet);
            return Content(JsonConvert.SerializeObject(parsedItems.ToArray()),"application/json");
        }

        


        [Authorize]
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

        [Authorize]
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