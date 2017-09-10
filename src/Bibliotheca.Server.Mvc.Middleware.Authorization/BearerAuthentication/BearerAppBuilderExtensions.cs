using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.BearerAuthentication
{
    public static class BearerAppBuilderExtensions
    {
        public static AuthenticationBuilder AddBearerAuthentication(this AuthenticationBuilder authenticationBuilder, Action<JwtBearerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return authenticationBuilder.AddJwtBearer(options);
        }

        public static IApplicationBuilder UseRewriteAccessTokenFronQueryToHeader(this IApplicationBuilder applicationBuilder)
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

            return applicationBuilder;
        }
    }
}