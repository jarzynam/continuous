using System.ServiceProcess;

namespace Continuous.Test.BasicService
{
    public partial class ContinuousBasicService : ServiceBase
    {
        private readonly FileWriter _writer;

        public ContinuousBasicService()
        {
            InitializeComponent();
            CanPauseAndContinue = true;

            _writer = new FileWriter();
        }

        protected override void OnStart(string[] args)
        {
            _writer.LogStart();
        }
        
        protected override void OnStop()
        {
            _writer.LogEnd();
        }

        protected override void OnPause()
        {
         _writer.LogPause();   
        }

        protected override void OnContinue()
        {
            _writer.LogContinue();
        }
    }
}
