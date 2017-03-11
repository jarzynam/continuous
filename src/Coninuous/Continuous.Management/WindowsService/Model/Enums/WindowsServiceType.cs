namespace Continuous.Management.WindowsService.Model
{
    public enum WindowsServiceType
    {
        KernelDriver,
        FileSystemDriver,
        Adapter,
        RecognizerDriver,
        OwnProcess,
        ShareProcess,
        InteractiveProcess
    }
}