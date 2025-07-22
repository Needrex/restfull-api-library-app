using System;

namespace RestApiApp.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
    }   
}