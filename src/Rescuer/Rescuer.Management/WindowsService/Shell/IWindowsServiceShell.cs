using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace Rescuer.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell : IDisposable
    {
        ServiceControllerStatus GetServiceStatus();
        bool ConnectToService(string serviceName);
        bool InstallService(string serviceName, string fullServicePath);
        bool UninstallService(string serviceName);        
        List<string> ErrorLog { get; set; }
        void ClearErrorLog();
        bool StopService();
        bool StartService();
    }
}