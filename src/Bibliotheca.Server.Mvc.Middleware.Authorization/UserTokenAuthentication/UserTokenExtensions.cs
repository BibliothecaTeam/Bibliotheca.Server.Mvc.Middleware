using System;
using Microsoft.AspNetCore.Authentication;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public static class UserTokenExtensions
    {
        public static AuthenticationBuilder AddUserToken(this AuthenticationBuilder builder, Action<UserTokenOptions> configureOptions)
        {
            return builder.AddScheme<UserTokenOptions, UserTokenAuthenticationHandler>(
                UserTokenSchema.Name, UserTokenSchema.Description, configureOptions);
        }
    }
}