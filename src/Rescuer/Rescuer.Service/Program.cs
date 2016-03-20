using System.ServiceProcess;

namespace Rescuer.Service
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
                new RescuerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
