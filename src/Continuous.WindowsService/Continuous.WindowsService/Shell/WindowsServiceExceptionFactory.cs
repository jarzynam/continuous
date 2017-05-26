using System;
using System.IO;
using Continuous.WindowsService.Resources;

namespace Continuous.WindowsService.Shell
{
    internal interface IWindowsServiceExceptionFactory
    {
        void FileNotFoundException(string path);
        void ServiceNotFoundException(string serviceName);
        void ServiceException(uint exceptionCode);
    }

    internal class WindowsServiceExceptionFactory : IWindowsServiceExceptionFactory
    {
        private readonly IWin32ServiceMessages _messages;

        public WindowsServiceExceptionFactory(IWin32ServiceMessages messages)
        {
            _messages = messages;
        }

        public void FileNotFoundException(string path)
        {
            throw new FileNotFoundException("Can't find file in path: " + path);
        }

        public void ServiceNotFoundException(string serviceName)
        {
            throw new InvalidOperationException("Can't find service with name: " + serviceName);
        }

        public void ServiceException(uint exceptionCode)
        {
            throw new InvalidOperationException("Error occured Reason: " + _messages.GetMessage(exceptionCode));
        }
    }
}