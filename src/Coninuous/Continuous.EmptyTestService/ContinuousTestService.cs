using System.ServiceProcess;

namespace Rescuer.Services.EmptyTestService
{
    public partial class ContinuousTestService : ServiceBase
    {        
        public ContinuousTestService()
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
