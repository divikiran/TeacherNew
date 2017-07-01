using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using InspectionWriterWebApi.Utilities;

namespace InspectionWriterWebApi.Handlers
{
    /// <summary>
    /// Handler that is configured as MessageHandler in App_Start/WebApiConfig and performs request authentication for any requests to methods/classes decorated with the <see cref="AuthorizeAttribute" />
    /// </summary>
    public class AuthenticationHandler : DelegatingHandler
    {
        /// <summary>
        /// Using HTTP Basic authentication
        /// </summary>
        private const string SCHEME = "Basic";

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var headers = request.Headers;

                if (headers.Authorization != null && SCHEME.Equals(headers.Authorization.Scheme))
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var creds = encoding.GetString(Convert.FromBase64String(headers.Authorization.Parameter));
                    var parts = creds.Split(':');
                    var username = parts[0].Trim();
                    var password = parts[1].Trim();

                    var userAccountGuid = await AuthenticateUser(username, password, cancellationToken);

                    if (userAccountGuid.HasValue)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password),
                        };

                        var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, SCHEME), });

                        Thread.CurrentPrincipal = principal;

                        if (HttpContext.Current != null)
                        {
                            // Storing the User Name and UserAccountID for use in methods that require it, rather than passing it as a parameter to web service calls
                            HttpContext.Current.User = principal;

                            HttpContext.Current.Items.Add("UserAccountGuid", userAccountGuid.Value);
                        }
                    }
                }

                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Per spec, Unauthorized requests should return the authentication scheme used.
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(SCHEME));
                }

                return response;
            }
            catch (Exception)
            {
                var response = request.CreateResponse(HttpStatusCode.Unauthorized);

                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(SCHEME));

                return response;
            }
        }

        /// <summary>
        /// Checks to see if the username/password submitted in the HTTP Basic authentication header are valid
        /// </summary>
        /// <param name="username">AMS username</param>
        /// <param name="password">AMS password</param>
        /// <param name="cancellationToken">Token used to cancel async request</param>
        /// <returns></returns>
        private static async Task<Guid?> AuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            const string sql = "SELECT UserAccountID FROM UserAccount WITH (NOLOCK) WHERE UserAccount = @Username AND Password = @Password";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    await cn.OpenAsync(cancellationToken);

                    return await cmd.ExecuteScalarAsync(cancellationToken) as Guid?;
                }
            }
        }
    }
}