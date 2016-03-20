using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Rescuer.Management;
using Rescuer.Management.Factory;


namespace Rescuer.Service
{
    public partial class RescuerService : ServiceBase
    {        
        private Timer _workTimer;
        private RescuerControllerFactory _controllerFactory;

        public RescuerService()
        {
            InitializeComponent();     
        }
        
        protected override void OnStart(string[] args)
        {
            _controllerFactory = new RescuerControllerFactory();

            var controller = _controllerFactory.Create();            
            
            var monitoredServices = GetMonitoredEntities("monitoredServices");

            var rescuers = controller.IntializeRescuers(monitoredServices);

            _workTimer = new Timer(p => controller.DoWork(rescuers), null, 5000, 5000);
        }

        protected override void OnStop()
        {
            _workTimer.Dispose();
            _controllerFactory.Dispose();
        }

        private string[] GetMonitoredEntities(string settingsKey)
        {
            var monitoredServices = ConfigurationManager.AppSettings[settingsKey].Split(',');
            return monitoredServices;
        }


    }
}
