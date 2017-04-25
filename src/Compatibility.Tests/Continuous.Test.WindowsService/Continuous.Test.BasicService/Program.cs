using System.ServiceProcess;

namespace Continuous.Test.BasicService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ContinuousBasicService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
