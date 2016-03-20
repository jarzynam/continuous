using System;

namespace Rescuer.Management.Rescuers.WindowsService.Exceptions
{
    public class ServiceRescueException : Exception
    {
        public ServiceRescueException()
        {

        }

        public ServiceRescueException(string message) : base(message)
        {

        }

        public ServiceRescueException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}