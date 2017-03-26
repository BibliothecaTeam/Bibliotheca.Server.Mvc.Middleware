using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public class UserTokenOptions : AuthenticationOptions
    {
        public UserTokenOptions() : base()
        {
            AuthenticationScheme = UserTokenDefaults.AuthenticationScheme;
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }

        public string AuthorizationUrl { get; set; }

        public string Realm { get; set; }
    }
}