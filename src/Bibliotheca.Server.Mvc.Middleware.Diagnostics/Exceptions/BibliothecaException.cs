using System;
using System.Net;

namespace Bibliotheca.Server.Mvc.Middleware.Diagnostics.Exceptions
{
    public class BibliothecaException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

        public BibliothecaException()
        {
        }

        public BibliothecaException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public BibliothecaException(string message) : base(message)
        {
        }

        public BibliothecaException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}