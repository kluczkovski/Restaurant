using System;

namespace Restaurant.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)  
        {
        }
    }
}
