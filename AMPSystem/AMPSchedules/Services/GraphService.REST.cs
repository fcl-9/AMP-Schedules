/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

// Microsoft.Graph

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AMPSchedules.TokenStorage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resources;

namespace AMPSchedules.Services
{
    public class GraphServiceREST : IGraphService
    {
        // Get the current aUser's email address from their profile.
        public async Task<string> GetMyEmailAddress()
        {
            const string endpoint  = "https://graph.microsoft.com/v1.0/me";
            const string parameter = "?$select=mail,userPrincipalName";

            string token = await UserTokenProvider.Instance.GetUserAccessTokenAsync();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage( HttpMethod.Get, endpoint + parameter ) )
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", token );
                    using ( HttpResponseMessage response = await client.SendAsync(request ) )
                    {
                        if ( response.StatusCode != HttpStatusCode.OK ) return string.Empty;

                        var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                        var address = ! string.IsNullOrEmpty( json.GetValue("mail").ToString() ) 
                            ? json.GetValue("mail").ToString()
                            : json.GetValue("userPrincipalName").ToString();
                        return address.Trim();
                    }
                }
            }
        }
        public async Task<byte[]> GetPhoto()
        {
            const string endpoint = "https://graph.microsoft.com/v1.0/me/photo/$value";

            string token = await UserTokenProvider.Instance.GetUserAccessTokenAsync();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage( HttpMethod.Get, endpoint ))
                {
                    request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "image/jpeg" ) );
                    request.Headers.Authorization=new AuthenticationHeaderValue( "Bearer", token );
                    using (HttpResponseMessage response = await client.SendAsync( request ))
                    {
                        if ( response.StatusCode != HttpStatusCode.OK ) return null;

                        if ( response.Content.Headers.ContentType.MediaType=="image/jpeg" )
                        {
                            return await response.Content.ReadAsByteArrayAsync();
                        }
                    }
                }
            }
            return null;
        }

        public async Task<byte[]> GetUserPhoto( string aUser)
        {
            string endpoint = $"https://graph.microsoft.com/v1.0/users/{aUser}/photo/$value";

            string token = await UserTokenProvider.Instance.GetUserAccessTokenAsync();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage( HttpMethod.Get, endpoint ))
                {
                    request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "image/jpeg" ) );
                    request.Headers.Authorization=new AuthenticationHeaderValue( "Bearer", token );
                    using (HttpResponseMessage response = await client.SendAsync( request ))
                    {
                        if ( response.StatusCode != HttpStatusCode.OK ) return null;

                        if (response.Content.Headers.ContentType.MediaType=="image/jpeg")
                        {
                            return await response.Content.ReadAsByteArrayAsync();
                        }
                    }
                }
            }
            return null;
        }

        // Send an email message from the current aUser.
        public async Task<string> SendEmail( IMessageRequest aMessage )
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me/sendMail";

            string token = await UserTokenProvider.Instance.GetUserAccessTokenAsync();

            using ( var client = new HttpClient() )
            {
                using ( var request = new HttpRequestMessage( HttpMethod.Post, endpoint ) )
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", token );
                    request.Content = new StringContent( JsonConvert.SerializeObject(aMessage), Encoding.UTF8, "application/json" );
                    using ( HttpResponseMessage response = await client.SendAsync(request) )
                    {
                        if ( ! response.IsSuccessStatusCode ) return response.ReasonPhrase;

                        return Resource.Graph_SendMail_Success_Result;
                    }
                }
            }
        }
    }
}
