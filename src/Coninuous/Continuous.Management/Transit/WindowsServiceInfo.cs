using System.Security.Policy;

namespace Rescuer.Management.Transit
{
    public class WindowsServiceInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public string StartMode { get; set; }

        public string ServiceType { get; set; }

        public int ProcessId { get; set; }

        public  string State { get; set; }

        public string Description { get; set; }

    }
}
