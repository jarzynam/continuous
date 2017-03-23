using Continuous.Management.WindowsServices.Model.Enums;

namespace Continuous.Management.WindowsServices.Model
{
    /// <summary>
    /// Model using to creating new windows service
    /// </summary>
    public class WindowsServiceConfiguration
    {
        /// <summary>
        /// Name of the service to install (max 256 characters)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the service. (max 256 characters)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Fully qualified path to the service binary file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Account name under which the service runs.
        /// LocalSystem as default
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Account domain. Local domain as default
        /// </summary>
        public string AccountDomain { get; set; }

        /// <summary>
        /// Account password
        /// </summary>
        public string AccountPassword { get; set; }

        /// <summary>
        /// Driver object name for kernel and system-level drivers
        /// Otherwise can be NULL
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Type of process which will be invoking this service. OwnProcess as default
        /// </summary>
        public WindowsServiceType Type { get; set;  } = WindowsServiceType.OwnProcess;

        /// <summary>
        /// Severity of the error if the Create method fails to start. 
        /// </summary>
        public WindowsServiceErrorControl ErrorControl { get; set; } = WindowsServiceErrorControl.Normal;

        /// <summary>
        /// Start mode of the Windows base service. Auto as default.
        /// </summary>
        public WindowsServiceStartMode StartMode { get; set; } = WindowsServiceStartMode.Automatic;

        /// <summary>
        /// If true, the service can create or communicate with windows on the desktop. False as default;
        /// </summary>
        public bool InteractWithDesktop { get; set; } = false;

    }
}
