using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public class SecureTokenAuthenticationHandler : ISecureTokenAuthenticationHandler
    {
        private HttpContext _context;
        private AuthenticationScheme _scheme;

        private readonly ISecureTokenOptions _secureTokenOptions;

        public SecureTokenAuthenticationHandler(ISecureTokenOptions secureTokenOptions)
        {
            _secureTokenOptions = secureTokenOptions;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            string authorization = _context.Request.Headers["Authorization"];
            string token = null;

            if (string.IsNullOrWhiteSpace(authorization))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (authorization.StartsWith($"{_scheme.Name} ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring($"{_scheme.Name} ".Length).Trim();
            }
            else
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Value for {_scheme.Name} scheme not exists."));
            }

            bool isValid = ValidateToken(token);
            if (isValid)
            {
                var identity = new ClaimsIdentity(_scheme.Name, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "SystemId"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "System"));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), _scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail($"{_scheme.Name} is invalid."));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            _context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            _context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{SecureTokenSchema.Name} realm=neutrino-api");
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

        private bool ValidateToken(string token)
        {
            var secureToken = _secureTokenOptions.SecureToken;
            return secureToken.Equals(token, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}