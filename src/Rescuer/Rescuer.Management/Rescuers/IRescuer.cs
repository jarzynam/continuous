using System;
using Rescuer.Management.Transit;

namespace Rescuer.Management.Rescuers
{
    public interface IRescuer : IDisposable
    {
        HealthStatus CheckHealth();
        void Rescue();
        void Connect(string serviceName);
        RescueStatus MonitorAndRescue();
        int RescueCounter { get;  }

        string ConnectedServiceName { get; }

    }
}