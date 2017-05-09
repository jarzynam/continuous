using Continuous.WindowsService.Shell;

namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Windows service start mode
    /// </summary>
    public enum WindowsServiceStartMode
    {
        /// <summary>
        /// Device driver will start by the operating system loader. This value is valid only for driver services
        /// </summary>
        Boot,
        /// <summary>
        /// Device driver will start by the operating system initialization process. This value is valid only for driver services
        /// </summary>
        System,
        /// <summary>
        /// Service will start automatically during system startup.
        /// </summary>
        Automatic,
        /// <summary>
        /// Service will start when a process calls the StartService method.
        /// </summary>
        /// <seealso cref="IWindowsServiceShell.Start"/>
        Manual,
        /// <summary>
        /// Indicates that the service is disabled, so that it cannot be started by a user or application.
        /// </summary>
        Disabled,
        /// <summary>
        /// Service will start automatically with short delay after system startup 
        /// </summary>
        AutomaticDelayedStart,
    }
}