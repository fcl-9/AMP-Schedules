/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Resources;

namespace AMPSchedules.TokenStorage
{
    public sealed class UserTokenProvider : IUserTokenProvider
    {

        // Properties used to get and manage an access token.
        private static readonly string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static readonly string AppId       = ConfigurationManager.AppSettings["ida:AppId"];
        private static readonly string AppSecret   = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static readonly string Scopes      = ConfigurationManager.AppSettings["ida:GraphScopes"];

        private UserTokenCache TokenCache { get; set; }

        #region Singleton

        private UserTokenProvider() { } 

        public static UserTokenProvider Instance { get; } = new UserTokenProvider();

        #endregion

        // Gets an access token. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            string signedInUserId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

            TokenCache = new UserTokenCache( signedInUserId );

            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                AppId, RedirectUri, new ClientCredential(AppSecret), TokenCache 
            );

            try
            {
                AuthenticationResult result = await cca.AcquireTokenSilentAsync(Scopes.Split(new char[] { ' ' }));
                return result.Token;
            }
            catch ( MsalSilentTokenAcquisitionException )
            {
                // Unable to retrieve the access token silently.

                HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
                throw new Exception(Resource.Error_AuthChallengeNeeded);
            }
        }

        private void Debug( TokenCache aTokenCache )
        {
            // See what's in the cache
            var cachedItems = aTokenCache.ReadItems(AppId); 
            foreach ( var item in cachedItems )
            {
                var token = item as TokenCacheItem;
                if ( token == null ) continue;

                System.Diagnostics.Trace.WriteLine( "-----------------------------------------------------------------------" );
                System.Diagnostics.Trace.WriteLine( string.Format( "    Authority : {0}", token.Authority ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     TenantId : {0}", token.TenantId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     ClientId : {0}", token.ClientId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "     UniqueId : {0}", token.UniqueId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "DisplayableId : {0}", token.DisplayableId ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "         Name : {0}", token.Name ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "       Scopes : {0}", string.Join( ", ", token.Scope ) ) );
                System.Diagnostics.Trace.WriteLine( string.Format( "    ExpiresOn : {0}", token.ExpiresOn ) );
                System.Diagnostics.Trace.WriteLine( "" );
                System.Diagnostics.Trace.WriteLine( string.Format( " Token => {0}", token.Token ) );
                System.Diagnostics.Trace.WriteLine( "" );
            }
        }
    }
}
