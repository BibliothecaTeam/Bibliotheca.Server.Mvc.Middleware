using Microsoft.AspNetCore.Authentication;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public class SecureTokenOptions : AuthenticationSchemeOptions
    {
        public string SecureToken { get; set; }
    }
}