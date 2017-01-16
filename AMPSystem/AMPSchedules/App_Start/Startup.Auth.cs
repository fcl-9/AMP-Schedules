using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Net.Mail;
using System.Threading.Tasks;
using AMPSchedules.TokenStorage;
using AMPSystem.Classes.LoadData;
using AMPSystem.Interfaces;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace AMPSchedules
{
    public partial class Startup
    {

        // The appId is used by the application to uniquely identify itself to Azure AD.
        // The appSecret is the application's password.
        // The redirectUri is where users are redirected after sign in and consent.
        // The graphScopes are the Microsoft Graph permission scopes that are used by this sample: User.Read Mail.Send

        private static readonly string TenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static readonly string AppId = ConfigurationManager.AppSettings["ida:AppId"];
        private static readonly string AppSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private static readonly string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static readonly string GraphScopes = ConfigurationManager.AppSettings["ida:GraphScopes"];

        public void ConfigureAuth(IAppBuilder app)
        {
            var authority = "https://login.microsoftonline.com/" + TenantId + "/v2.0";

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {

                    // The `Authority` represents the Microsoft v2.0 authentication and authorization service.
                    // The `Scope` describes the permissions that your app will need. 
                    // See https://azure.microsoft.com/documentation/articles/active-directory-v2-scopes/                    

                    ClientId = AppId,
                    Authority = authority,
                    PostLogoutRedirectUri = RedirectUri,
                    RedirectUri = RedirectUri,
                    Scope = "openid email profile offline_access " + GraphScopes,

                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;
                            string signedInUserId = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            ConfidentialClientApplication cca = new ConfidentialClientApplication(
                                AppId, RedirectUri, new ClientCredential(AppSecret), new UserTokenCache(signedInUserId));
                            string[] scopes = GraphScopes.Split(new char[] { ' ' });

                            AuthenticationResult result = await cca.AcquireTokenByAuthorizationCodeAsync(scopes, code);
                            var mail = new MailAddress(result.User.DisplayableId);

                            var user = mail.User;

                            IDataReader dataReader = new FileData();
                            Repository.Instance.DataReader = dataReader;
                            Repository.Instance.CleanRepository();
                            Repository.Instance.GetData(user);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}