﻿using System.ServiceProcess;

namespace Rescuer.Services.EmptyTestService
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
                new RescuerTestService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
