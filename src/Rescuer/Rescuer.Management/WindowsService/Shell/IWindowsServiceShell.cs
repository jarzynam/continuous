using System.Collections.Generic;

namespace Rescuer.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell
    {
        string GetServiceStatus();
        bool ConnectToService(string serviceName);
        bool InstallService(string serviceName, string fullServicePath);
        bool UninstallService(string serviceName);
        List<string> ErrorLog { get; set; }
        void ClearErrorLog();
    }
}