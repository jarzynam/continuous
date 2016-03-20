using Rescuer.Management.Rescuers;

namespace Rescuer.Management.Controller
{
    public class RescuerController : IRescuerController
    {                
        private readonly IRescuerFactory _factory;

        public RescuerController(IRescuerFactory factory)
        {
            _factory = factory;
        }

        public IRescuer[] IntializeRescuers(string[] monitoredEntities)
        {
            var rescuers = new IRescuer[monitoredEntities.Length];
            for (int i = 0; i < rescuers.Length; i++)
            {
                rescuers[i] = _factory.Create();
                rescuers[i].Connect(monitoredEntities[i]);
            }

            return rescuers;
        }
     

        public void DoWork(IRescuer[] rescuers)
        {
            for (int i = 0; i < rescuers.Length; i++)
            {
                rescuers[i].MonitorAndRescue();
            }
        }

        
    }
}