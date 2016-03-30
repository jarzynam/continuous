using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Rescuer.Management.Controller;

namespace Rescuer.Service
{
    public partial class RescuerService : ServiceBase
    {                
        private Configuration _configuration;

        private readonly RescuerControllerFactory _controllerFactory;
        private readonly ILogger _logger;
        private Task _task;
        private readonly CancellationTokenSource _tokenSource;

        public RescuerService()
        {
            InitializeComponent();            
            _logger = LogManager.GetCurrentClassLogger();
            _controllerFactory = new RescuerControllerFactory();

            _tokenSource = new CancellationTokenSource();
        }
      
        protected override void OnStart(string[] args)
        {            
            try
            {
                _logger.Info("starting service");

                _configuration = new Configuration();
                var controller = _controllerFactory.Create();                
                
                _logger.Info($"Found {_configuration.MonitoredEntities.Length} services to monitor"); 
         
                var rescuers = controller.IntializeRescuers(_configuration.MonitoredEntities);

                _task = Task.Factory.StartNew(() =>
                {
                    while (!_tokenSource.IsCancellationRequested)
                    {                        
                        controller.DoWork(rescuers);                        

                        _task.Wait(TimeSpan.FromSeconds(5));
                    }                                        
                }, _tokenSource.Token);

            }
            catch (Exception ex)
            {                
                _logger.Fatal($"Unable to work due to error: {ex}");
                throw;
            }            
        }

        

        protected override void OnStop()
        {
            _tokenSource.Cancel(false);
            try
            {
                _task.Wait();
            }
            catch (AggregateException agx)
            {
                foreach (var exception in agx.InnerExceptions)
                {
                    _logger.Error(exception);
                }
            }
            finally
            {
                _tokenSource.Dispose();                
                _controllerFactory.Dispose();
                _logger.Info("service stopped");
            }
            
            
            
        }
    }
}
