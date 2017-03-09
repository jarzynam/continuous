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
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly TimeSpan _timeout;


        public WindowsServiceShell()
        {
            _timeout = TimeSpan.FromSeconds(5);
            _executor = new ScriptExecutor();

            _scripts = new ScriptsBoundle();
        }

        public ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            using (var service = new ServiceController(serviceName))
            {
                return service.Status;
            }
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

        public bool StopService(string serviceName)
        {
            using (var service = new ServiceController(serviceName))
            {
                if (!service.CanStop)
                    return false;

                service.Stop();

                service.WaitForStatus(ServiceControllerStatus.Stopped, _timeout);

                return true;
            }
        }

        public bool StartService(string serviceName)
        {
            using (var service = new ServiceController(serviceName))
            {
                if (service.Status == ServiceControllerStatus.Running)
                    return false;

                service.Start();

                service.WaitForStatus(ServiceControllerStatus.Running, _timeout);

                return true;
            }
        }

        public WindowsServiceInfo GetService(string serviceName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.GetService, parameters).FirstOrDefault();

            if (result == null) return null;

            return new WindowsServiceInfo
            {
                Name = result.Properties["Name"].Value as string,
                DisplayName = result.Properties["DisplayName"].Value as string,
                Description = result.Properties["Description"].Value as string,
                ProcessId = (result.Properties["ProcessId"].Value as int?).GetValueOrDefault(),
                UserName = result.Properties["StartName"].Value as string,
                ServiceType = result.Properties["ServiceType"].Value as string,
                StartMode = result.Properties["StartMode"].Value as string,
                State = result.Properties["State"].Value as string,
                Status = result.Properties["Status"].Value as string
            };
        }

        public void ChangeUser(string serviceName, string userName, string password, string domain = ".")
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("newAccount", string.Join(@"\", domain, userName)),
                new CommandParameter("newPassword", password)
            };

            var result = _executor.Execute(_scripts.ChangeUser, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result)
        {
            var returnValue = result.FirstOrDefault()?.Properties["ReturnValue"].Value as int?;

            if (returnValue.GetValueOrDefault() != 0)
                throw new InvalidOperationException("Cannont change user. Reason: " + returnValue.GetValueOrDefault());
        }

    }
}