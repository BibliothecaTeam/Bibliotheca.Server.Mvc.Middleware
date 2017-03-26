using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public static class UserTokenAppBuilderExtension
    {
        public static IApplicationBuilder UseUserTokenAuthentication(this IApplicationBuilder applicationBuilder, UserTokenOptions options)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return applicationBuilder.UseMiddleware<UserTokenMiddleware>(Options.Create(options));
        }
    }
}