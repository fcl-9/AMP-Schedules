﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using Microsoft.Graph;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    //Commented for tests only!!!!!!!!!!!!!![Authorize]
    public class HomeController : TemplateController
    {
        readonly GraphService _graphService = new GraphService();

        public ActionResult Index()
        {
            return View($"Graph");
        }
     
        // API Controller
        public async Task<ActionResult> CalendarDefaultData()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message});
            }
        }

        // Get the current user's email address from their profile.
        public async Task<ActionResult> GetMyEmailAddress()
        {
            try
            {
                // Initialize the GraphServiceClient.
                var graphClient = SDKHelper.GetAuthenticatedClient();

                // Get the current user's email address. 
                ViewBag.Email = await _graphService.GetMyEmailAddress(graphClient);
                return View($"Graph");
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        // Send mail on behalf of the current user.
        public async Task<ActionResult> SendEmail()
        {
            if (string.IsNullOrEmpty(Request.Form["email-address"]))
            {
                ViewBag.Message = Resource.Graph_SendMail_Message_GetEmailFirst;
                return View($"Graph");
            }

            // Build the email message.
            var message = _graphService.BuildEmailMessage(Request.Form["recipients"], Request.Form["subject"]);
            try
            {
                // Initialize the GraphServiceClient.
                var graphClient = SDKHelper.GetAuthenticatedClient();

                // Send the email.
                await _graphService.SendEmail(graphClient, message);

                // Reset the current user's email address and the status to display when the page reloads.
                ViewBag.Email = Request.Form["email-address"];
                ViewBag.Message = Resource.Graph_SendMail_Success_Result;
                return View($"Graph");
            }
            catch (ServiceException se)
            {
                if (se.Error.Code == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }
    }
}
