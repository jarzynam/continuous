namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Windows service running process type
    /// </summary>
    public enum WindowsServiceType
    {

        /// <summary>
        /// Kernel device driver such as a hard disk or other low-level hardware device driver
        /// </summary>
        KernelDriver = 0x1,

        /// <summary>
        /// file system driver, which is also a Kernel device driver
        /// </summary>
        FileSystemDriver = 0x2,

        /// <summary>
        /// service for a hardware device that requires its own driver
        /// </summary>
        Adapter = 0x4,

        /// <summary>
        /// file system driver used during startup to determine the file systems present on the system.
        /// </summary>
        RecognizerDriver = 0x8,
        /// <summary>
        ///  service runs in a process by itself
        /// </summary>
        OwnProcess = 0x10,

        /// <summary>
        /// service that shares a process with one or more other services
        /// </summary>
        ShareProcess = 0x20,

        /// <summary>
        /// service that can communicate with the desktop
        /// </summary>
        InteractiveProcess = 0x100,

        /// <summary>
        /// Unknown service type
        /// </summary>
        Unknown


    }
}