using Continuous.Management.WindowsServices.Model.Enums;

namespace Continuous.Management.WindowsServices.Model
{
    public class WindowsServiceInstallModel
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
        /// Fully qualified path to the executable file that implements the service
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Account name under which the service runs.
        /// Can be null for local system account
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Account domain. Can be NULL or "." for local domain
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        /// Driver object name for kernel and system-level drivers
        /// Otherwise can be NULL
        /// </summary>
        public string DriverName { get; set; }
        
        /// <summary>
        /// Type of services provided to processes that call them
        /// </summary>
        public WindowsServiceType Type { get; set; }

        /// <summary>
        /// Severity of the error if the Create method fails to start. 
        /// </summary>
        public WindowsServiceErrorControl ErrorControl { get; set; }

        /// <summary>
        /// Start mode of the Windows base service
        /// </summary>
        public WindowsServiceStartMode StartMode { get; set; }

        /// <summary>
        /// If true, the service can create or communicate with windows on the desktop.
        /// </summary>
        public bool InteractWithDesktop { get; set; }

    }
}
