using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace Rescuer.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell : IDisposable
    {
        List<string> ErrorLog { get; set; }        
        ServiceControllerStatus GetServiceStatus();
        void ConnectToService(string serviceName);
        void InstallService(string serviceName, string fullServicePath);
        void UninstallService(string serviceName);
        void ClearErrorLog();
        bool StopService();
        bool StartService();
    }
}