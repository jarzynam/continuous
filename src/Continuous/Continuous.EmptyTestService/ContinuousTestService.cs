using System.ServiceProcess;

namespace Continuous.Services.EmptyTestService
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
