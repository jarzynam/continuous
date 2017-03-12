namespace Continuous.Management.WindowsServices.Model.Enums
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