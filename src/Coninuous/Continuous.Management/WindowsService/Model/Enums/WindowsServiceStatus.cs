namespace Continuous.Management.WindowsService.Model.Enums
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