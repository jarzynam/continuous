using Continuous.Management.WindowsServices.Model.Enums;

namespace Continuous.Management.WindowsServices.Model
{
    public class WindowsServiceInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public WindowsServiceStatus Status { get; set; }

        public string UserName { get; set; }

        public string UserDomain { get; set; }

        public WindowsServiceStartMode StartMode { get; set; }

        public WindowsServiceType WindowsServiceType { get; set; }

        public int ProcessId { get; set; }

        public WindowsServiceState State { get; set; }

        public string Description { get; set; }

    }
}
