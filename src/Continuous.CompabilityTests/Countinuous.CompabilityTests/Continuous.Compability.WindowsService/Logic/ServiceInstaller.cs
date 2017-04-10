using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Continuous.Management;
using Continuous.WindowsService;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Shell;
using NUnit.Framework;

namespace Continuous.Compability.WindowsService.Logic
{
	public class  ServiceInstaller : IDisposable
    {
        private readonly IWindowsServiceShell _shell;
       
		public string ServicePath { get;  set; }

		private readonly List<string> _installedServices;
        private readonly ReaderWriterLockSlim _lock;

        public ServiceInstaller()
        {
			_lock = new ReaderWriterLockSlim();
           
            _shell = new ContinuousManagementFactory().WindowsServiceShell();

            var location = AppDomain.CurrentDomain.BaseDirectory;
            ServicePath = Path.Combine(location, "Resources",
             "Continuous.CompabilityTests.BasicService.exe");

			_installedServices = new List<string>();
        }

        public void InstallService(string serviceName)
        {
            _shell.Install(serviceName, ServicePath);

            AddInstalledServiceName(serviceName);
        }

        public void InstallService(WindowsServiceConfiguration configuration)
        {
            _shell.Install(configuration);

            AddInstalledServiceName(configuration.Name);
        }

        public void UninstallService(string serviceName)
        {
            _shell.Uninstall(serviceName);

			_lock.EnterWriteLock();
            try
            {
                _installedServices.Remove(serviceName);
            }
            finally
            {
                _lock.EnterWriteLock();
            }
           
        }

        public void Dispose()
        {
			_lock.EnterReadLock();
            try
            {
                foreach (var service in _installedServices)
                {
                    try
                    {
                        UninstallService(service);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        private void AddInstalledServiceName(string serviceName)
        {
            _lock.EnterWriteLock();
            try
            {
                _installedServices.Add(serviceName);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

}