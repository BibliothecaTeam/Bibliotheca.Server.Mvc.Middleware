using System;
using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.BearerAuthentication
{
    public static class BearerAppBuilderExtensions
    {
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder applicationBuilder, JwtBearerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            RewriteAccessTokenFronQueryToHeader(applicationBuilder);
            return applicationBuilder.UseJwtBearerAuthentication(options);
        }

        private static void RewriteAccessTokenFronQueryToHeader(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                {
                    if (context.Request.Query.ContainsKey("access_token"))
                    {
                        var token = context.Request.Query["access_token"];
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
                        }
                    }
                }
                await next.Invoke();
            });
        }
    }
}