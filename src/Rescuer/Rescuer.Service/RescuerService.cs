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
        private readonly IContainer _container;        
        private Timer _workTimer;

        public RescuerService()
        {
            InitializeComponent();
            var builder = new ContainerBuilder();

            builder.RegisterModule<RescuerManagementModule>();

            _container = builder.Build();

        }
        
        protected override void OnStart(string[] args)
        {
            var factory = _container.ResolveKeyed<IRescuerFactory>(RescuerFactoryType.WindowsServiceRescuer);
            var controller = _container.Resolve<IRescuerController>(new NamedParameter("factory", factory));
            
            var monitoredServices = GetMonitoredEntities("monitoredServices");

            var rescuers = controller.IntializeRescuers(monitoredServices);

            _workTimer = new Timer(p => controller.DoWork(rescuers), null, 5000, 5000);
        }

        protected override void OnStop()
        {
            _workTimer.Dispose();
            _container.Dispose();
        }

        private string[] GetMonitoredEntities(string settingsKey)
        {
            var monitoredServices = ConfigurationManager.AppSettings[settingsKey].Split(',');
            return monitoredServices;
        }


    }
}
