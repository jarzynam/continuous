using System.Collections.Generic;

namespace Rescuer.Management.WindowsService.Shell
{
    public interface IWindowsServiceShell
    {
        string GetServiceStatus(string serviceName);
        bool CheckExisting(string serviceName);
        bool InstallService(string serviceName);
        bool UninstallService(string serviceName);
        List<string> ErrorLog { get; set; }
        void ClearErrorLog();
    }
}