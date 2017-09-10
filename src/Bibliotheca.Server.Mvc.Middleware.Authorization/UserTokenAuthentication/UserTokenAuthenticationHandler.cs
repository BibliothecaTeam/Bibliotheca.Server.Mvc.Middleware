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

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public class UserTokenAuthenticationHandler : IUserTokenAuthenticationHandler
    {
        private HttpContext _context;
        private AuthenticationScheme _scheme;

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            string authorization = _context.Request.Headers["Authorization"];
            string token = null;

            if (string.IsNullOrWhiteSpace(authorization))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            if (authorization.StartsWith($"{_scheme.Name} ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring($"{_scheme.Name} ".Length).Trim();
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            var contextOptions = _context.RequestServices.GetService<IUserTokenConfiguration>();
            var authorizationUrl = contextOptions.GetAuthorizationUrl();
            if(string.IsNullOrWhiteSpace(authorizationUrl))
            {
                return AuthenticateResult.Fail($"{_scheme.Name} authentication failed. Authorization server was not specified.");
            }

            var user = await GetUserByTokenAsync(token, authorizationUrl);
            if (user != null)
            {
                var identity = new ClaimsIdentity(_scheme.Name, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), _scheme.Name);

                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return AuthenticateResult.Fail($"{_scheme.Name} authentication failed. Credentials are invalid.");
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            _context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{UserTokenSchema.Name} realm=neutrino-api");
            return Task.FromResult(0);
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            return Task.FromResult(0);
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _scheme = scheme;
            _context = context;

            return Task.FromResult(0);
        }

        private async Task<UserDto> GetUserByTokenAsync(string token, string authorizationUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add(UserTokenSchema.Name, token);

            var address = Path.Combine(authorizationUrl, "accessToken");
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