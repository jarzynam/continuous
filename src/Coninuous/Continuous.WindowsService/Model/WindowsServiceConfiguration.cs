namespace Continuous.WindowsService.Model
{
    /// <summary>
    /// Model using to creating new windows service
    /// </summary>
    public class WindowsServiceConfiguration : WindowsServiceConfigurationForUpdate
    {
        /// <summary>
        /// Name of the service to install (max 256 characters)
        /// </summary>
        public string Name { get; set; }


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

    }
}
