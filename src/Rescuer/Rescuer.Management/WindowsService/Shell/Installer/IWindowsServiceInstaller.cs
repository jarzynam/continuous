namespace Rescuer.Management.WindowsService.Shell.Installer
{
    public interface IWindowsServiceInstaller
    {
        bool Install(string serviceName);
        bool Uninstall(string serviceName);
    }
}