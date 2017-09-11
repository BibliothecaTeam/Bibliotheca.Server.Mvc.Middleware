

using System;
using Microsoft.AspNetCore.Authentication;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public static class SecureTokenExtensions
    {
        public static AuthenticationBuilder AddSecureToken(this AuthenticationBuilder builder, Action<SecureTokenOptions> configureOptions)
        {
            return builder.AddScheme<SecureTokenOptions, SecureTokenAuthenticationHandler>(
                SecureTokenSchema.Name, SecureTokenSchema.Description, configureOptions);
        }
    }
}