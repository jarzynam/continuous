using Continuous.Management.WindowsServices.Shell;

namespace Continuous.Management.WindowsServices.Model.Enums
{
    /// <summary>
    /// Windows service start modes
    /// </summary>
    public enum WindowsServiceStartMode
    {
        /// <summary>
        /// Device driver started by the operating system loader. This value is valid only for driver services
        /// </summary>
        Boot,
        /// <summary>
        /// Device driver started by the operating system initialization process. This value is valid only for driver services
        /// </summary>
        System,
        /// <summary>
        /// Service to be started automatically by the Service Control Manager during system startup.
        /// </summary>
        Auto,
        /// <summary>
        /// Service to be started by the Service Control Manager when a process calls the StartService method.
        /// </summary>
        /// <seealso cref="IWindowsServiceShell.Start"/>
        Manual,
        /// <summary>
        /// Service that can no longer be started.
        /// </summary>
        Disabled
    }
}