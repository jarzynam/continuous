using System.ServiceProcess;

namespace Continuous.Services.EmptyTestService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new ContinuousTestService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
