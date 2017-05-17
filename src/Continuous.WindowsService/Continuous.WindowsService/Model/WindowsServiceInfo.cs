using System.Collections.Generic;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell.Extensions;

namespace Continuous.WindowsService.Model
{
    /// <summary>
    /// Base informations about windows service
    /// </summary>
    public class WindowsServiceInfo : WindowsServiceInfoExtensions
    {
        public WindowsServiceInfo()
        {
            InitializeBase(this);
        }

        /// <summary>
        /// Service name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Service display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Account name - LocalSystem as default
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Account domain - '.' as local domain
        /// </summary>
        public string AccountDomain { get; set; }

        /// <summary>
        /// Service description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of process using by this service
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Windows service status
        /// </summary>
        public WindowsServiceStatus Status { get; set; }

        /// <summary>
        /// Service start mode
        /// </summary>
        public WindowsServiceStartMode StartMode { get; set; }

        /// <summary>
        /// Type of the process responsible for running this service
        /// </summary>
        public WindowsServiceType Type { get; set; }

        /// <summary>
        /// Current state of this service
        /// </summary>
        public WindowsServiceState State { get; set; }

        /// <summary>
        /// Serverity of the error throwed in service installation process
        /// </summary>
        public WindowsServiceErrorControl ErrorControl { get; set; }

        /// <summary>
        /// Determines if service can interact with desktop
        /// </summary>
        public bool InteractWithDesktop { get; set; }

        /// <summary>
        /// Fully qualified path to the service binary file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// System Error Code, if equals to 1066,
        /// the value of exit code is in ServiceSpecificExitCode property
        /// </summary>
        public uint ExitCode { get; set; }

        /// <summary>
        /// Custom exit code from service
        /// </summary>
        public uint ServiceSpecificExitCode { get; set; }

        /// <summary>
        /// Determines if service can be stopped now
        /// </summary>
        public bool CanStop { get; set; }

        /// <summary>
        /// Determines if service can be paused now
        /// </summary>
        public bool CanPause { get; set; }

        /// <summary>
        /// List of services names that must start before this service will start
        /// </summary>
        public ICollection<string> ServiceDependencies { get; set; }
    }
}
