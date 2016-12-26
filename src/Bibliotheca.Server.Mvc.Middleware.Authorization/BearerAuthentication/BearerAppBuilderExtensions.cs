using System;
using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization
{
    public static class BearerAppBuilderExtensions
    {
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder applicationBuilder, JwtBearerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return applicationBuilder.UseJwtBearerAuthentication(options);
        }
    }
}