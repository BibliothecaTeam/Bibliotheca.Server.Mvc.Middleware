using System;
using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public static class UserTokenAppBuilderExtension
    {
        public static IApplicationBuilder UseUserTokenAuthentication(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            return applicationBuilder.UseMiddleware<UserTokenMiddleware>();
        }
    }
}