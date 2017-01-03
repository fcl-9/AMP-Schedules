/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

// Microsoft.Graph

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AMPSchedules.TokenStorage;
using Microsoft.Graph;
using IMessageRequest = AMPSchedules.Services.IMessageRequest;

namespace AMPSchedules.Services
{
    public class GraphServiceGraph : IGraphService
    {
        // Get an authenticated Microsoft Graph Service client.
        public static GraphServiceClient GetAuthenticatedClient()
        {
            return new GraphServiceClient(
                new DelegateAuthenticationProvider( async (requestMessage) => {
                        string accessToken = await UserTokenProvider.Instance.GetUserAccessTokenAsync();
                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue( "bearer", accessToken );
                    } ) );
        }

        // Get the current user's email address from their profile.
        public async Task<string> GetMyEmailAddress()
        {
            // Get the current user. 
            // The app only needs the user's email address, so select the mail and userPrincipalName properties.
            // If the mail property isn't defined, userPrincipalName should map to the email for all account types. 
            User me = await GetAuthenticatedClient().Me.Request().Select( "mail, userPrincipalName" ).GetAsync();
            return me.Mail ?? me.UserPrincipalName;
        }
        public Task<byte[]> GetPhoto()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetUserPhoto( string aUser )
        {
            throw new NotImplementedException();
        }

        // Send an email message from the current user.
        public async Task<string> SendEmail( IMessageRequest aMessage )
        {
            BodyType contentType;
            Enum.TryParse( aMessage.Message.Body.ContentType, true, out contentType );

            var recipients = new List<Microsoft.Graph.Recipient>();
            foreach ( var recipient in aMessage.Message.ToRecipients )
            {
                recipients.Add( new Microsoft.Graph.Recipient()
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = recipient.EmailAddress.Address
                    }
                } );
            }

            var message = new Microsoft.Graph.Message()
            {
                Body = new Microsoft.Graph.ItemBody {
                    Content = aMessage.Message.Body.Content,
                    ContentType = contentType
                },
                Subject = aMessage.Message.Subject,
                ToRecipients = recipients
            };

            await GetAuthenticatedClient().Me.SendMail( message, aMessage.SaveToSentItems ).Request().PostAsync();

            return string.Empty;
        }
    }
}
