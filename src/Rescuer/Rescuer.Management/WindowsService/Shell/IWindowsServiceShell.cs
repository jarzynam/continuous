namespace Rescuer.Management
{
    public interface IWindowsServiceShell
    {
        string GetServiceStatus(string serviceName);
    }
}