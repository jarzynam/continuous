namespace Continuous.Management.WindowsService.Model.Enums
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