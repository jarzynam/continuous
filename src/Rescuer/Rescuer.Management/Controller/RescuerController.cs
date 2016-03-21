using System;
using System.Text;
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
                if (String.IsNullOrWhiteSpace(monitoredEntities[i]))
                {
                    var entitiesString = ToFlatString(monitoredEntities);
                    throw new ArgumentException($"monitored entity name can't be null or empty! FailedIndex: {i} Array: [{entitiesString}]");
                }

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

        private static string ToFlatString(string[] array)
        {
            var builder = new StringBuilder();

            foreach (var entity in array)
            {
                builder.Append(entity);
                builder.Append(",");
            }
            var str = builder.ToString();

            return str.Remove(str.Length - 1, 1);
        }
    }
}