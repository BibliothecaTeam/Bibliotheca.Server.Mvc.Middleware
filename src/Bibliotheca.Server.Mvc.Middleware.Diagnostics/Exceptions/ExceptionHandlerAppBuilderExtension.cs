using Microsoft.AspNetCore.Builder;

namespace Bibliotheca.Server.Mvc.Middleware.Diagnostics.Exceptions
{
    public static class ExceptionHandlerAppBuilderExtension
    {
        /// <summary>
        /// Handler which catch exceptions, logs them and returns proper Http staus code.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>Builder.</returns>
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
