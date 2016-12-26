using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bibliotheca.Server.Mvc.Middleware.Authorization
{
    public class SecureTokenMiddleware : AuthenticationMiddleware<SecureTokenOptions>
    {
        public SecureTokenMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, UrlEncoder encoder, IOptions<SecureTokenOptions> options)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        protected override AuthenticationHandler<SecureTokenOptions> CreateHandler()
        {
            return new SecureTokenHandler();
        }
    }
}