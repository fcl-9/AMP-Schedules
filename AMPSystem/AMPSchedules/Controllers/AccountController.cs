/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AMPSchedules.Helpers;
using AMPSchedules.TokenStorage;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // Signal OWIN to send an authorization request to Azure.
                HttpContext.GetOwinContext().Authentication.Challenge(
                  new AuthenticationProperties { RedirectUri = "/" },
                  OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Clear the token cache, sign out the GraphServiceClient, and end the session with the web app.  
        /// </summary>
        public void SignOut()
        {
            if (Request.IsAuthenticated)
            {
                // Get the user's token cache and clear it.
                string userObjectId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                SessionTokenCache tokenCache = new SessionTokenCache(userObjectId, HttpContext);
                tokenCache.Clear(userObjectId);
            }

            SDKHelper.SignOutClient();

            // Send an OpenID Connect sign-out request. 
            HttpContext.GetOwinContext().Authentication.SignOut(
              CookieAuthenticationDefaults.AuthenticationType);
            Response.Redirect("/");
        }
    }
}