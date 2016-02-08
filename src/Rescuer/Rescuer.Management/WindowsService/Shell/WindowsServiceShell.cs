using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;

namespace Rescuer.Management.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        public List<string> ErrorLog { get; set; }

        private ServiceController _service;

        public WindowsServiceShell()
        {
            ErrorLog = new List<string>();
        }
        
        public string GetServiceStatus()
        {
            if(_service == null)
                throw new InvalidOperationException("Can't get service status before connect to the windows service");

            return _service.Status.ToString();
        }

        public bool ConnectToService(string serviceName)
        {
            _service = ServiceController.GetServices()
            .FirstOrDefault(s => s.ServiceName == serviceName);

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

        private void GetErrors(PowerShell powershell)
        {
            ErrorLog = powershell.Streams.Error.ReadAll().Select(p => p.Exception.ToString()).ToList();
        }

        public bool UninstallService(string serviceName)
        {
            using (var powershell = PowerShell.Create(RunspaceMode.NewRunspace))
            {
                powershell.AddScript($"$service = Get-WmiObject -Class Win32_Service -Filter \"Name = '{serviceName}'\"; if($service){{$service.delete()}}");

                powershell.Invoke();
                
                GetErrors(powershell);

                return !powershell.HadErrors;
            }
        }

        public void ClearErrorLog()
        {
            ErrorLog = new List<string>();
        }
    }
}