using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Diagnostics
{
    public static class ExceptionHandlerAppBuilderExtension
    {
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
