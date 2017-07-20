using System;

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
            throw new InvalidOperationException("Can't find user with name: " + userName);
        }

    }
}