using System;
using Rescuer.Management.Transit;

namespace Rescuer.Management.Rescuers
{
    public interface IRescuer : IDisposable
    {
        HealthStatus CheckHealth();
        bool Rescue();
        void Connect(string serviceName);
        void MonitorAndRescue();

    }
}