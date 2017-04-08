namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Specifies error severity and the action taken if this service fails to start
    /// </summary>
    public enum WindowsServiceErrorControl
    {

        /// <summary>
        /// The startup program logs the error but continues the startup operation
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// The startup program logs the error and displays a message but continues the startup operation.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// The startup program logs the error. If the "last known good" configuration is started, the startup operation continues.
        /// Otherwise, the system is restarted with the "last known good" configuration
        /// </summary>
        Severe = 2,

        /// <summary>
        /// The startup program logs the error, if possible. If the "last known good" configuration is started, the system startup is cancelled. 
        /// Otherwise, the system is restarted with the "last known good" configuration.
        /// </summary>
        Critical = 3,

        /// <summary>
        /// error control is unknown
        /// </summary>
        Unknown
      
    }
}