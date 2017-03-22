namespace Continuous.Management.WindowsServices.Model.Enums
{
    /// <summary>
    /// Windows service running process type
    /// </summary>
    public enum WindowsServiceType
    {

        /// <summary>
        /// Kernel device driver such as a hard disk or other low-level hardware device driver
        /// </summary>
        KernelDriver = 1,

        /// <summary>
        /// file system driver, which is also a Kernel device driver
        /// </summary>
        FileSystemDriver = 2,

        /// <summary>
        /// service for a hardware device that requires its own driver
        /// </summary>
        Adapter = 4,

        /// <summary>
        /// file system driver used during startup to determine the file systems present on the system.
        /// </summary>
        RecognizerDriver = 8,
        /// <summary>
        ///  service runs in a process by itself
        /// </summary>
        OwnProcess = 16,

        /// <summary>
        /// service that shares a process with one or more other services
        /// </summary>
        ShareProcess = 32,

        /// <summary>
        /// service that can communicate with the desktop
        /// </summary>
        InteractiveProcess = 256
    }
}