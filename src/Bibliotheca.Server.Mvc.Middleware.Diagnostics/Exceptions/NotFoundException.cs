using System;
using System.Net;

namespace Bibliotheca.Server.Mvc.Middleware.Diagnostics.Exceptions
{
    public class NotFoundException : BibliothecaException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}