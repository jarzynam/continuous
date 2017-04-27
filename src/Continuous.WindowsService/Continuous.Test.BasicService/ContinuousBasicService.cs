using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Continuous.Test.BasicService
{
    public partial class ContinuousBasicService : ServiceBase
    {
        private readonly FileWriter _writer;
        private readonly int _delayTime;

        public ContinuousBasicService()
        {
            InitializeComponent();
            
            _writer = new FileWriter();

            CanPauseAndContinue = true;

            _delayTime = int.Parse(ConfigurationManager.AppSettings["DelayTime"]);
        }

        protected override void OnStart(string[] args)
        {
            RequestAdditionalTime(_delayTime);
            Thread.Sleep(_delayTime);
            _writer.LogStart();
        }
        
        protected override void OnStop()
        {
            RequestAdditionalTime(_delayTime);
            Thread.Sleep(_delayTime);
            _writer.LogEnd();
        }

        protected override void OnPause()
        {
            RequestAdditionalTime(_delayTime);
            Thread.Sleep(_delayTime);
            _writer.LogPause();
        }

        protected override void OnContinue()
        {
            RequestAdditionalTime(_delayTime);
            Thread.Sleep(_delayTime);
            _writer.LogContinue();
        }
    }
}
