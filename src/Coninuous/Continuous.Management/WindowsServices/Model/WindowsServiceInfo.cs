using Continuous.Management.WindowsServices.Model.Enums;

namespace Continuous.Management.WindowsServices.Model
{
    public class WindowsServiceInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public WindowsServiceStatus Status { get; set; }

        public string AccountName { get; set; }

        public string AccountDomain { get; set; }

        public WindowsServiceStartMode StartMode { get; set; }

        public WindowsServiceType Type { get; set; }

        public int ProcessId { get; set; }

        public WindowsServiceState State { get; set; }

        public string Description { get; set; }

    }
}
