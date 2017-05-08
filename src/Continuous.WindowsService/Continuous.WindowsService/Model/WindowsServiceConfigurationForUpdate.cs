using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Model
{
    /// <summary>
    /// Model using for update existing windows service
    /// </summary>
    public class WindowsServiceConfigurationForUpdate
    {
        /// <summary>
        /// Display name of the service. (max 256 characters)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description of windows service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Fully qualified path to the service binary file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Type of process which will be invoking this service. OwnProcess as default
        /// </summary>
        public WindowsServiceType? Type { get; set; }

        /// <summary>
        /// Severity of the error if the Create method fails to start. 
        /// </summary>
        public WindowsServiceErrorControl? ErrorControl { get; set; }

        /// <summary>
        /// Start mode of the Windows base service. Auto as default.
        /// </summary>
        public WindowsServiceStartMode? StartMode { get; set; }

        /// <summary>
        /// If true, the service can create or communicate with windows on the desktop. False as default;
        /// </summary>
        public bool? InteractWithDesktop { get; set; } 
    }
}
