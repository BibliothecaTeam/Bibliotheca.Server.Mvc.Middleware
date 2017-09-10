using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.SecureTokenAuthentication
{
    public static class SecureTokenExtension
    {
        public static AuthenticationOptions AddSecureTokenAuthentication(this AuthenticationOptions authenticationOptions, IServiceCollection services)
        {
            services.AddScoped<ISecureTokenAuthenticationHandler, SecureTokenAuthenticationHandler>();

            authenticationOptions.AddScheme(SecureTokenSchema.Name, builder => {
                builder.DisplayName = SecureTokenSchema.Description;
                builder.HandlerType = typeof(ISecureTokenAuthenticationHandler);
            });

            return authenticationOptions;
        }
    }
}