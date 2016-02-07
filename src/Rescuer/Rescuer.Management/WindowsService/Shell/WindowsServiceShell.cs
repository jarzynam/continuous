using System;
using System.Collections.Generic;

namespace Rescuer.Management.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        public string GetServiceStatus(string serviceName)
        {
            throw new NotImplementedException();
        }

        public bool CheckExisting(string serviceName)
        {
            throw new NotImplementedException();
        }

        public bool InstallService(string serviceName)
        {
            throw new NotImplementedException();
        }

        public bool UninstallService(string serviceName)
        {
            throw new NotImplementedException();
        }

        public List<string> ErrorLog { get; set; }
        public void ClearErrorLog()
        {
            throw new NotImplementedException();
        }
    }
}