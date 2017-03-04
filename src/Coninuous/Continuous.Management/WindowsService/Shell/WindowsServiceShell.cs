using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.ServiceProcess;

namespace Rescuer.Management.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly TimeSpan _timeout;

        private ServiceController _service;
        private readonly ScriptExecutor _executor;
        private readonly string _scriptsPath;

        private const string UninstallServiceScriptName = "UninstallService.ps1";
        private const string InstallServiceScriptName = "InstallService.ps1";

        public WindowsServiceShell()
        {
            ErrorLog = new List<string>();
            _timeout = TimeSpan.FromSeconds(5);
            _executor = new ScriptExecutor();

            var currentPath = AppDomain.CurrentDomain.BaseDirectory;

            _scriptsPath = Path.Combine(currentPath, "WindowsService", "Scripts");
        }

        public List<string> ErrorLog { get; set; }

        public ServiceControllerStatus GetServiceStatus()
        {
            ThrowExceptionIfNotConnectedToService();

            _service.Refresh();

            return _service.Status;
        }

        public bool ConnectToService(string serviceName)
        {
            _service = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == serviceName);

            if (_service == null)
            {
                ErrorLog.Add("can't find service with name " + serviceName);
            }
            return _service != null;
        }

        public void InstallService(string serviceName, string fullServicePath)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName),
                new CommandParameter(nameof(fullServicePath), fullServicePath)
            };
            var path = GetPath(InstallServiceScriptName);

            _executor.Execute(path, parameters);
        }

        public void UninstallService(string serviceName)
        {
            
                var parameters = new List<CommandParameter> {new CommandParameter(nameof(serviceName), serviceName)};
                var path = GetPath(UninstallServiceScriptName);

                _executor.Execute(path, parameters);
              
        }


        public void ClearErrorLog()
        {
            ErrorLog = new List<string>();
        }

        public bool StopService()
        {
            ThrowExceptionIfNotConnectedToService();

            if (!_service.CanStop)
            {
                ErrorLog.Add("service can't be stopped after start");
                return false;
            }

            _service.Stop();

            _service.WaitForStatus(ServiceControllerStatus.Stopped, _timeout);

            return true;
        }

        public bool StartService()
        {
            ThrowExceptionIfNotConnectedToService();

            if (_service.Status == ServiceControllerStatus.Running)
                return false;

            _service.Start();

            _service.WaitForStatus(ServiceControllerStatus.Running, _timeout);

            return true;
        }

        public void Dispose()
        {
            _service?.Dispose();
        }

        private void ThrowExceptionIfNotConnectedToService()
        {
            if (_service == null)
                throw new InvalidOperationException("Can't get service status before connect to the windows service");
        }

        private void GetErrors(PowerShell powershell)
        {
            ErrorLog = powershell.Streams.Error.ReadAll().Select(p => p.Exception.ToString()).ToList();
        }

        private string GetPath(string scriptName)
        {
            return Path.Combine(_scriptsPath, scriptName);
        }
    }
}