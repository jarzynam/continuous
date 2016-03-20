using System.Configuration;

namespace Rescuer.Service
{
    public class Configuration
    {
        private const string MonitoringEntitiesKey = "MonitoredEntities";

        public Configuration()
        {
            MonitoredEntities = GetArrayFromSetting(MonitoringEntitiesKey);
        }

        public string[] MonitoredEntities { get; private set; }

        private string[] GetArrayFromSetting(string settingsKey)
        {
            var monitoredServices = ConfigurationManager.AppSettings[settingsKey].Split(',');
            return monitoredServices;
        }
    }
}