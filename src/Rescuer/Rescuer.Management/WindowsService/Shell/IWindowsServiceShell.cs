namespace Rescuer.Management.WindowsService
{
    public interface IWindowsServiceShell
    {
        string GetServiceStatus(string serviceName);
        bool CheckExisting(string isAny);
    }
}