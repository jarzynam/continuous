namespace Continuous.Management.WindowsServices.Model.Enums
{
    public enum WindowsServiceErrorControl
    {
        UserIsNotNotifed = 0,
        UserIsNotifed = 1,
        SystemIsRestartedWithLastGoodConfiguration = 2,
        SystemAttemptsToStartWithGoodConfiguration = 3
    }
}