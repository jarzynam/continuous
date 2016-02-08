using System.ServiceProcess;

namespace Rescuer.Services.EmptyTestService
{
    public partial class RescuerTestService : ServiceBase
    {        
        public RescuerTestService()
        {
            InitializeComponent();            
        }

        protected override void OnStart(string[] args)
        {            
            
        }

        protected override void OnStop()
        {
            
        }
    }
}
