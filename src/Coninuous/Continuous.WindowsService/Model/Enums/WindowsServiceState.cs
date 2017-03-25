namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Current state of the base service.
    /// </summary>
    public enum WindowsServiceState
    {
        Stopped,
        StartPending,
        StopPending,
        Running,
        ContinuePending,
        PausePending,
        Paused,
        Unknown   
    }
}