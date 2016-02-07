using System;

namespace Rescuer.Management.WindowsService
{
    public class WindowsServiceRescuer : IWindowsServiceRescuer
    {
        public HealthStatus CheckHealth()
        {
            throw new NotImplementedException();
        }
        
        public void ConnectToService(string serviceName)
        {
            throw new NotImplementedException();
        }
    }
}