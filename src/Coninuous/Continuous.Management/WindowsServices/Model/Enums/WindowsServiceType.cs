namespace Continuous.Management.WindowsServices.Model.Enums
{
    /// <summary>
    /// Windows service running process type
    /// </summary>
    public enum WindowsServiceType
    {
        KernelDriver = 1,
        FileSystemDriver = 2,
        Adapter = 4,
        RecognizerDriver = 8,
        OwnProcess = 16,
        ShareProcess = 32,
        InteractiveProcess = 256
    }
}