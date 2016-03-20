using Rescuer.Management.Rescuers;

namespace Rescuer.Management.Controller
{
    public interface IRescuerController
    {
        IRescuer[] IntializeRescuers(string[] monitoredEntities);
        void DoWork(IRescuer[] rescuers);
    }
}