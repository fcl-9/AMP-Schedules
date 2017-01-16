using System.Net.Http.Headers;
using AMPSchedules.TokenStorage;
using Microsoft.Graph;

namespace AMPSchedules.Services
{
    public class GraphServiceHelper
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
    }
}