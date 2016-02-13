using System;

namespace Rescuer.Management
{
    public interface IRescuer : IDisposable
    {
        HealthStatus CheckHealth();
        bool Rescue();
        void Connect(string serviceName);
        void MonitorAndRescue();

    }
}