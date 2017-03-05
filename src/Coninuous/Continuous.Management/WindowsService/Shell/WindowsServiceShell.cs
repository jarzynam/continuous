using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.ServiceProcess;
using Continuous.Management.Common;
using Continuous.Management.WindowsService.Model;

namespace Continuous.Management.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly TimeSpan _timeout;

        private ServiceController _service;
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;

      
        public WindowsServiceShell()
        {
            ErrorLog = new List<string>();
            _timeout = TimeSpan.FromSeconds(5);
            _executor = new ScriptExecutor();

            _scripts = new ScriptsBoundle();
        }

        public List<string> ErrorLog { get; set; }

        public ServiceControllerStatus GetServiceStatus()
        {
            ThrowExceptionIfNotConnectedToService();

            _service.Refresh();

            return _service.Status;
        }

        public void ConnectToService(string serviceName)
        {
            _service = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == serviceName);

           ThrowExceptionIfNotConnectedToService();
        }

        public void InstallService(string serviceName, string fullServicePath)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName),
                new CommandParameter(nameof(fullServicePath), fullServicePath)
            };

            _executor.Execute(_scripts.InstallService, parameters);
        }

        public void UninstallService(string serviceName)
        { 
            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName)
            };
            
            _executor.Execute(_scripts.UninstallService, parameters);   
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

        public void ChangeUser(string userName, string password, string domain = ".")
        {
            ThrowExceptionIfNotConnectedToService();

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", _service.ServiceName),
                new CommandParameter("newAccount", String.Join(@"\", domain, userName)),
                new CommandParameter("newPassword", password)
            };

            var result = _executor.Execute(_scripts.ChangeUser, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        public WindowsServiceInfo GetService()
        {
            ThrowExceptionIfNotConnectedToService();

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", _service.ServiceName)
            };

            var result = _executor.Execute(_scripts.GetService, parameters).FirstOrDefault();

            if (result == null) return null;

            return new WindowsServiceInfo
            {
                Name = result.Properties["Name"].Value as String,
                DisplayName = result.Properties["DisplayName"].Value as String,
                Description = result.Properties["Description"].Value as String,
                ProcessId = (result.Properties["ProcessId"].Value as int?).GetValueOrDefault(),
                UserName = result.Properties["StartName"].Value as string,
                ServiceType = result.Properties["ServiceType"].Value as string,
                StartMode = result.Properties["StartMode"].Value as string,
                State = result.Properties["State"].Value as string,
                Status = result.Properties["Status"].Value as string
            };

        }

        public void Dispose()
        {
            _service?.Dispose();
        }

        private void ThrowExceptionIfNotConnectedToService()
        {
            if (_service == null)
                throw new InvalidOperationException("Service is not connected");
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result)
        {
            var returnValue = result.FirstOrDefault()?.Properties["ReturnValue"].Value as int?;

            if (returnValue.GetValueOrDefault() != 0)
                throw new InvalidOperationException("Cannont change user. Reason: " + returnValue.GetValueOrDefault());
        }
    }
}