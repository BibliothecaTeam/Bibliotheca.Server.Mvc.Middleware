using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public class SecureTokenAuthenticationHandler : AuthenticationHandler<SecureTokenOptions>
    {
        public SecureTokenAuthenticationHandler(
            IOptionsMonitor<SecureTokenOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
        : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Logger.LogDebug($"{Scheme.Name} HandleAuthenticateAsync...");

            string authorization = Context.Request.Headers["Authorization"];
            string token = null;

            if (string.IsNullOrWhiteSpace(authorization))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (authorization.StartsWith($"{Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring($"{Scheme.Name} ".Length).Trim();
            }
            else
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Value for {Scheme.Name} scheme not exists."));
            }

            bool isValid = ValidateToken(token);
            if (isValid)
            {
                var identity = new ClaimsIdentity(Scheme.Name, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "SystemId"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "System"));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail($"{Scheme.Name} is invalid."));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug($"{Scheme.Name} HandleChallengeAsync...");

            Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            Context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{SecureTokenSchema.Name} realm=Bibliotheca");
            return Task.FromResult(0);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Logger.LogDebug($"{Scheme.Name} HandleForbiddenAsync...");
            return Task.FromResult(0);
        }

        private bool ValidateToken(string token)
        {
            var secureToken = Options.SecureToken;
            return secureToken.Equals(token, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}