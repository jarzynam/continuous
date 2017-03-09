using System;
using System.ServiceProcess;
using Continuous.Management.WindowsService.Model;

namespace Continuous.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell
    {
        ServiceControllerStatus GetServiceStatus(string serviceName);
        void InstallService(string serviceName, string fullServicePath);
        void UninstallService(string serviceName);
        bool StopService(string serviceName);
        bool StartService(string serviceName);
        void ChangeUser(string serviceName, string userName, string password, string domain = ".");
        WindowsServiceInfo GetService(string serviceName);
    }
}