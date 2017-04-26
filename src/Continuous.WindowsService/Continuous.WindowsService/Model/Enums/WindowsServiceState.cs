namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Current state of the base service.
    /// </summary>
    public enum WindowsServiceState
    {
        /// <summary>
        /// Service has stopped
        /// </summary>
        Stopped,
        /// <summary>
        /// Service is executing start command
        /// </summary>
        StartPending,
        /// <summary>
        /// Service is executing stop command
        /// </summary>
        StopPending,
        /// <summary>
        /// Service is running
        /// </summary>
        Running,
        /// <summary>
        /// Service is executing continue command
        /// </summary>
        ContinuePending,
        /// <summary>
        /// Service is executing pause command
        /// </summary>
        PausePending,
        /// <summary>
        /// Service has paused
        /// </summary>
        Paused,
        /// <summary>
        /// Unable to determine in which state is service
        /// </summary>
        Unknown   
    }
}