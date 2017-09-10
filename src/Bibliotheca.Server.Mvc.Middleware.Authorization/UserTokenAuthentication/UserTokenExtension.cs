using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization.UserTokenAuthentication
{
    public static class UserTokenExtension
    {
        public static AuthenticationOptions AddSecureTokenAuthentication(this AuthenticationOptions authenticationOptions, IServiceCollection services)
        {
            services.AddScoped<IUserTokenAuthenticationHandler, UserTokenAuthenticationHandler>();

            authenticationOptions.AddScheme(UserTokenSchema.Name, builder => {
                builder.DisplayName = UserTokenSchema.Description;
                builder.HandlerType = typeof(IUserTokenAuthenticationHandler);
            });

            return authenticationOptions;
        }
    }
}