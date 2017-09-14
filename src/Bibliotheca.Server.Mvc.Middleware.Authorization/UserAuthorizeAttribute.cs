using System;
using Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication;
using Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public UserAuthorizeAttribute() : base()
        {
            AuthenticationSchemes = $"{SecureTokenSchema.Name}, {UserTokenSchema.Name}, {JwtBearerDefaults.AuthenticationScheme}";
        }
    }
}