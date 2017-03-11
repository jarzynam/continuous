namespace Continuous.Management.WindowsService.Model
{
    public enum WindowsServiceStatus
    {
        Ok,
        Error,
        Degraded,
        Unknown,
        PredFail,
        Starting,
        Stopping,
        Service,
        Stressed,
        NonRecover,
        NoContact,
        LostComm
    }
}