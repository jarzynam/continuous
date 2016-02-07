namespace Rescuer.Management
{
    public interface IRescuer
    {
        HealthStatus CheckHealth();        
        void ConnectToService(string serviceName);
    }
}