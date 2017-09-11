using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public class UserTokenAuthenticationHandler : AuthenticationHandler<UserTokenOptions>
    {
        protected UserTokenAuthenticationHandler(
            IOptionsMonitor<UserTokenOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Logger.LogDebug($"{Scheme.Name} HandleAuthenticateAsync...");

            string authorization = Context.Request.Headers["Authorization"];
            string token = null;

            if (string.IsNullOrWhiteSpace(authorization))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            if (authorization.StartsWith($"{Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring($"{Scheme.Name} ".Length).Trim();
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            var contextOptions = Context.RequestServices.GetService<IUserTokenConfiguration>();
            var authorizationUrl = contextOptions.GetAuthorizationUrl();
            if(string.IsNullOrWhiteSpace(authorizationUrl))
            {
                return AuthenticateResult.Fail($"{Scheme.Name} authentication failed. Authorization server was not specified.");
            }

            var user = await GetUserByTokenAsync(token, authorizationUrl);
            if (user != null)
            {
                var identity = new ClaimsIdentity(Scheme.Name, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);

                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return AuthenticateResult.Fail($"{Scheme.Name} authentication failed. Credentials are invalid.");
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug($"{Scheme.Name} HandleChallengeAsync...");

            Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            Context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{UserTokenSchema.Name} realm=neutrino-api");
            return Task.FromResult(0);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug($"{Scheme.Name} HandleForbiddenAsync...");
            return Task.FromResult(0);
        }

        private async Task<UserDto> GetUserByTokenAsync(string token, string authorizationUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(UserTokenSchema.Name, token);

            var address = authorizationUrl.AppendPathSegment("accessToken");
            var response = await client.GetAsync(address);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDto>(responseString);
                return user;
            }

            return null;
        }
    }
}