using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.ServiceProcess;
using Continuous.Management.Common;
using Continuous.Management.WindowsServices.Model;
using Continuous.Management.WindowsServices.Resources;

namespace Continuous.Management.WindowsServices.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly TimeSpan _timeout;

        private readonly IWin32ServiceMessages _messages;
        private readonly Mapper _mapper;


        public WindowsServiceShell()
        {
            _timeout = TimeSpan.FromSeconds(5);
            _executor = new ScriptExecutor();

            _scripts = new ScriptsBoundle();
            _messages = new Win32ServiceMessages();
            _mapper = new Mapper();
        }

        public ServiceControllerStatus GetStatus(string serviceName)
        {
            using (var service = new ServiceController(serviceName))
            {
                return service.Status;
            }
        }

        public void Install(string serviceName, string fullServicePath)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName),
                new CommandParameter(nameof(fullServicePath), fullServicePath)
            };

            _executor.Execute(_scripts.InstallService, parameters);
        }

        public void Uninstall(string serviceName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName)
            };

            var result = _executor.Execute(_scripts.UninstallService, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        public bool Stop(string serviceName)
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

        public bool Start(string serviceName)
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
    
        public WindowsServiceInfo Get(string serviceName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.GetService, parameters).FirstOrDefault();

            if (result == null) return null;

            return _mapper.Map(result);
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

        private void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result)
        {
            var returnValue = result.FirstOrDefault()?.Properties["ReturnValue"].Value as int?;

            if (returnValue.GetValueOrDefault() != 0)
                throw new InvalidOperationException("Error occured Reason: " + _messages.GetMessage(returnValue.GetValueOrDefault()));
        }

    }
}