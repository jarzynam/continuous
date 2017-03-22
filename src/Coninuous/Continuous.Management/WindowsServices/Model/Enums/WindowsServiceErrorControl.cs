namespace Continuous.Management.WindowsServices.Model.Enums
{
    /// <summary>
    /// How system is handling with service installation errors
    /// </summary>
    public enum WindowsServiceErrorControl
    {

        /// <summary>
        /// User is not notified
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// User is notified, usually by message box
        /// </summary>
        Normal = 1,

        /// <summary>
        /// System is restarded with last good configuration
        /// </summary>
        Severe = 2,

        /// <summary>
        /// System attempts to restart with a good configuration.
        /// If the service fails to start a second time, startup fails.
        /// </summary>
        Critical = 3,

        /// <summary>
        /// error control is unknown
        /// </summary>
        Unknown
      
    }
}