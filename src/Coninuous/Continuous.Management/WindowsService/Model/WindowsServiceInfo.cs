using Continuous.Management.WindowsService.Model.Enums;

namespace Continuous.Management.WindowsService.Model
{
    public class WindowsServiceInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public WindowsServiceStatus Status { get; set; }

        public string UserName { get; set; }

        public WindowsServiceStartMode StartMode { get; set; }

        public WindowsServiceType WindowsServiceType { get; set; }

        public int ProcessId { get; set; }

        public WindowsServiceState State { get; set; }

        public string Description { get; set; }

    }
}
