namespace Continuous.Management.WindowsService.Model
{
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