/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using Resources;

namespace AMPSchedules.Controllers
{
    public class HomeController : TemplateController
    {
        private readonly GraphService graphService = new GraphService();

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
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }

        [Authorize]
        // Get the current user's email address from their profile.
        public async Task<ActionResult> GetMyEmailAddress()
        {
            try
            {
                // Get an access token.
                var accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                // Get the current user's email address. 
                ViewBag.Email = await graphService.GetMyEmailAddress(accessToken);
                return View("Graph");
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
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
            var email = graphService.BuildEmailMessage(Request.Form["recipients"], Request.Form["subject"]);
            try
            {
                // Get an access token.
                var accessToken = SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                // Send the email.
                ViewBag.Message = await graphService.SendEmail(accessToken, email);

                // Reset the current user's email address and the status to display when the page reloads.
                ViewBag.Email = Request.Form["email-address"];
                return View("Graph");
            }
            catch (Exception e)
            {
                if (e.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error",
                    new {message = Resource.Error_Message + Request.RawUrl + ": " + e.Message});
            }
        }
    }
}