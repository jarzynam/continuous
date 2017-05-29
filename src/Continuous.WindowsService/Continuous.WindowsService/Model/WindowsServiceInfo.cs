using System.Collections.Generic;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell.Extensions;

namespace Continuous.WindowsService.Model
{
    /// <summary>
    /// Base information about windows service
    /// </summary>
    public class WindowsServiceInfo : WindowsServiceInfoExtensions
    {
        /// <summary>
        /// Create empty instance
        /// </summary>
        public WindowsServiceInfo()
        {
            InitializeBase(this);
        }

        /// <summary>
        /// Create instance with existing service data. Throw exception if service not found
        /// </summary>
        /// <param name="serviceName">Existing windows service name</param>
        public WindowsServiceInfo(string serviceName)
        {
            Name = serviceName;

            InitializeBase(this);
            Refresh();
        }

        /// <summary>
        /// Service name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Service display name
        /// </summary>
        public string DisplayName { get; internal set; }

        /// <summary>
        /// Account name - LocalSystem as default
        /// </summary>
        public string AccountName { get; internal set; }

        /// <summary>
        /// Account domain - '.' as local domain
        /// </summary>
        public string AccountDomain { get; internal set; }

        /// <summary>
        /// Service description
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Id of process using by this service
        /// </summary>
        public int ProcessId { get; internal set; }

        /// <summary>
        /// Windows service status
        /// </summary>
        public WindowsServiceStatus Status { get; internal set; }

        /// <summary>
        /// Service start mode
        /// </summary>
        public WindowsServiceStartMode StartMode { get; internal set; }

        /// <summary>
        /// Type of the process responsible for running this service
        /// </summary>
        public WindowsServiceType Type { get; internal set; }

        /// <summary>
        /// Current state of this service
        /// </summary>
        public WindowsServiceState State { get; internal set; }

        /// <summary>
        /// Serverity of the error throwed in service installation process
        /// </summary>
        public WindowsServiceErrorControl ErrorControl { get; internal set; }

        /// <summary>
        /// Determines if service can interact with desktop
        /// </summary>
        public bool InteractWithDesktop { get; internal set; }

        /// <summary>
        /// Fully qualified path to the service binary file
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// System Error Code, if equals to 1066,
        /// the value of exit code is in ServiceSpecificExitCode property
        /// </summary>
        public uint ExitCode { get; internal set; }

        /// <summary>
        /// Custom exit code from service
        /// </summary>
        public uint ServiceSpecificExitCode { get; internal set; }

        /// <summary>
        /// Determines if service can be stopped now
        /// </summary>
        public bool CanStop { get; internal set; }

        /// <summary>
        /// Determines if service can be paused now
        /// </summary>
        public bool CanPause { get; internal set; }

        /// <summary>
        /// List of services names that must start before this service will start
        /// </summary>
        public ICollection<string> ServiceDependencies { get; internal set; }
    }
}
