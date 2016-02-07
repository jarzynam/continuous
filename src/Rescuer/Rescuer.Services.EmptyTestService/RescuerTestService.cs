using System.IO;
using System.ServiceProcess;

namespace Rescuer.Services.EmptyTestService
{
    public partial class RescuerTestService : ServiceBase
    {
        private readonly string _path;

        public RescuerTestService()
        {
            InitializeComponent();
            _path = Path.Combine(Directory.GetCurrentDirectory(), "im_working.txt");
        }

        protected override void OnStart(string[] args)
        {            
            File.Create(_path);
        }

        protected override void OnStop()
        {
            File.Delete(_path);
        }
    }
}
