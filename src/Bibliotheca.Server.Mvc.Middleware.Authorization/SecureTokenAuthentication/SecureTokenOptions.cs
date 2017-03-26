using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public class SecureTokenOptions : AuthenticationOptions
    {
        public SecureTokenOptions() : base()
        {
            AuthenticationScheme = SecureTokenDefaults.AuthenticationScheme;
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }

        public string SecureToken { get; set; } 

        public string Realm { get; set; }
    }
}