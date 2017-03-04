using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;

namespace Rescuer.Management.Rescuers.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly TimeSpan _timeout;

        private ServiceController _service;

        public WindowsServiceShell()
        {
            ErrorLog = new List<string>();
            _timeout = TimeSpan.FromSeconds(5);
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

        public bool InstallService(string serviceName, string fullServicePath)
        {
            using (var powershell = PowerShell.Create(RunspaceMode.NewRunspace))
            {
                powershell.AddScript($"New-Service -Name {serviceName} -BinaryPathName {fullServicePath}");

                powershell.Invoke();

                GetErrors(powershell);

                return !powershell.HadErrors;
            }
        }

        public bool UninstallService(string serviceName)
        {
            using (var powershell = PowerShell.Create(RunspaceMode.NewRunspace))
            {
                powershell.AddScript(
                    $"$service = Get-WmiObject -Class Win32_Service -Filter \"Name = '{serviceName}'\";" +
                    $"$id = $service | select -expand ProcessId;" +
                    $" if($id){{( Get-Process -Id $id).Kill()}} ;" +
                    $" if($service){{$service.delete()}}");

                powershell.Invoke();

                GetErrors(powershell);

                return !powershell.HadErrors;
            }
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
    }
}