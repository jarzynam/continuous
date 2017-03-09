using System;
using System.ServiceProcess;
using Continuous.Management.WindowsService.Model;

namespace Continuous.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell
    {
        ServiceControllerStatus GetStatus(string serviceName);
        void Install(string serviceName, string fullServicePath);
        void Uninstall(string serviceName);
        bool Stop(string serviceName);
        bool Start(string serviceName);
        void ChangeUser(string serviceName, string userName, string password, string domain = ".");
        WindowsServiceInfo Get(string serviceName);
    }
}