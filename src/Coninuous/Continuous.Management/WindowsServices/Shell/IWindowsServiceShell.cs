using System.ServiceProcess;
using Continuous.Management.WindowsServices.Model;

namespace Continuous.Management.WindowsServices.Shell
{
    public interface IWindowsServiceShell
    {
        ServiceControllerStatus GetStatus(string serviceName);
        void Install(string serviceName, string fullServicePath);
        void Install(WindowsServiceInstallModel model);
        void Uninstall(string serviceName);
        bool Stop(string serviceName);
        bool Start(string serviceName);
        void ChangeUser(string serviceName, string userName, string password, string domain = ".");
        WindowsServiceInfo Get(string serviceName);
    }
}