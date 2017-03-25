namespace Continuous.WindowsService.Model.Enums
{
    /// <summary>
    /// Current status of the object. Should be 'Ok' or one of the other statuses
    /// </summary>
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