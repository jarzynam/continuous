namespace Rescuer.Management
{
    public interface IRescuerFactory
    {
        IRescuer Create(string serviceName, RescuerType rescuerType);
    }
}