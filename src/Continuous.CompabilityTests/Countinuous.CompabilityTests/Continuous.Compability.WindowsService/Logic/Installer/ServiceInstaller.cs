using System;
using System.IO;
using Continuous.Management;
using Continuous.WindowsService;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Shell;

namespace Continuous.Compability.WindowsService.Logic.Installer
{
	public class  ServiceInstaller : Installer
    {
        private readonly IWindowsServiceShell _shell;
       
		public string ServicePath { get;  set; }

        public ServiceInstaller()
        {
            _shell = new ContinuousManagementFactory().WindowsServiceShell();

            var location = AppDomain.CurrentDomain.BaseDirectory;
            ServicePath = Path.Combine(location, "Resources",
                "Continuous.CompabilityTests.BasicService.exe");
        }

        public void InstallService(string serviceName)
        {
            _shell.Install(serviceName, ServicePath);

            AddInstance(serviceName);
        }

        public void InstallService(WindowsServiceConfiguration configuration)
        {
            _shell.Install(configuration);

            AddInstance(configuration.Name);
        }


        private void UninstallService(string serviceName)
        {
            _shell.Uninstall(serviceName);

            RemoveInstance(serviceName);
        }

        protected override void Uninstall(string instanceName)
        {
            UninstallService(instanceName);
        }
    }

}