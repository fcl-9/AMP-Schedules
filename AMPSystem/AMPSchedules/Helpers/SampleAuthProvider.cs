/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AMPSchedules.TokenStorage;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Resources;

namespace AMPSchedules.Helpers
{
    public sealed class SampleAuthProvider : IAuthProvider
    {
        private readonly string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private readonly string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];

        // Properties used to get and manage an access token.
        private readonly string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private readonly string scopes = ConfigurationManager.AppSettings["ida:GraphScopes"];

        private SampleAuthProvider()
        {
        }

        private SessionTokenCache tokenCache { get; set; }

        public static SampleAuthProvider Instance { get; } = new SampleAuthProvider();

        // Get an access token. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            var signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            tokenCache = new SessionTokenCache(
                signedInUserID,
                HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase);
            //var cachedItems = tokenCache.ReadItems(appId); // see what's in the cache

            var cca = new ConfidentialClientApplication(
                appId,
                redirectUri,
                new ClientCredential(appSecret),
                tokenCache);

            try
            {
                var result = await cca.AcquireTokenSilentAsync(scopes.Split(' '));
                return result.Token;
            }

            // Unable to retrieve the access token silently.
            catch (MsalSilentTokenAcquisitionException)
            {
                HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties {RedirectUri = "/"},
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

                throw new Exception(Resource.Error_AuthChallengeNeeded);
            }
        }
    }
}