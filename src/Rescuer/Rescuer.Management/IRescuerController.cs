namespace Rescuer.Management
{
    public interface IRescuerController
    {
        IRescuer[] IntializeRescuers(string[] monitoredEntities);
        void DoWork(IRescuer[] rescuers);
    }
}