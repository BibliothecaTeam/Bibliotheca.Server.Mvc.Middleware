using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization
{
    public static class SecureTokenAppBuilderExtension
    {
        public static IApplicationBuilder UseSecureTokenAuthentication(this IApplicationBuilder applicationBuilder, SecureTokenOptions options)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return applicationBuilder.UseMiddleware<SecureTokenMiddleware>(Options.Create(options));
        }
    }
}