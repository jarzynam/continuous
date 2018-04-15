using System;
using System.Management.Automation;

namespace Continuous.User.Users
{
    internal interface IUserExceptionFactory
    {
        void UserNotFoundException(string user);
    }

    internal class WindowsServiceExceptionFactory : IUserExceptionFactory
    {
        public void UserNotFoundException(string userName)
        {
            throw new MethodInvocationException("Can't find user with name: " + userName);
        }

    }
}