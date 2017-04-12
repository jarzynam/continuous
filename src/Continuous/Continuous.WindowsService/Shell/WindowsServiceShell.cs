﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.ServiceProcess;
using Continuous.Management.Common;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Resources;

namespace Continuous.WindowsService.Shell
{
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly IScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;

        private readonly TimeSpan _timeout;

        private readonly IWin32ServiceMessages _messages;
        private readonly Mapper _mapper;


        public WindowsServiceShell()
        {
            _timeout = TimeSpan.FromSeconds(5);

            _executor = new ScriptExecutor(GetType());
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
            ThrowIfCantFindFile(fullServicePath);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName),
                new CommandParameter(nameof(fullServicePath), fullServicePath)
            };

            _executor.Execute(_scripts.InstallService, parameters);
        }

        public void Install(WindowsServiceConfiguration config)
        {
            ThrowIfCantFindFile(config.Path);

            var startName = GetStartName(config);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", config.Name),
                new CommandParameter("displayName", config.DisplayName),
                new CommandParameter("errorControl", (byte) config.ErrorControl),
                new CommandParameter("startMode",  config.StartMode),
                new CommandParameter("serviceType", (byte) config.Type),
                new CommandParameter("desktopInteract", config.InteractWithDesktop),
                new CommandParameter("fullServicePath", config.Path),
                new CommandParameter("startPassword", config.AccountPassword),
                new CommandParameter("startName", startName),
            };

            var result = _executor.Execute(_scripts.InstallServiceWithParameters, parameters);

            ThrowServiceExceptionIfNecessary(result);

        }

        public void Update(string serviceName, WindowsServiceConfigurationForUpdate config)
        {
            ThrowIfCantFindService(serviceName);

            if(config.Path != null)
                ThrowIfCantFindFile(config.Path);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("displayName", config.DisplayName),
                new CommandParameter("errorControl", (byte) config.ErrorControl),
                new CommandParameter("startMode",  config.StartMode),
                new CommandParameter("serviceType", (byte) config.Type),
                new CommandParameter("desktopInteract", config.InteractWithDesktop),
                new CommandParameter("fullServicePath", config.Path)
            };

            var result = _executor.Execute(_scripts.UpdateServiceWithParameters, parameters);

            ThrowServiceExceptionIfNecessary(result);
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

            return result == null ? null : _mapper.Map(result);
        }

        public List<WindowsServiceInfo> GetAll()
        {
            var parameters = new List<CommandParameter>();

            var result = _executor.Execute(_scripts.GetAllServices, parameters);

            return result.Select(p => _mapper.Map(p)).ToList();
        }


        public void ChangeAccount(string serviceName, string accountName, string password, string domain = ".")
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("newAccount", string.Join(@"\", domain, accountName)),
                new CommandParameter("newPassword", password)
            };

            var result = _executor.Execute(_scripts.ChangeAccount, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        private void ThrowServiceExceptionIfNecessary(ICollection<PSObject> results)
        {
            var result = results.FirstOrDefault();

            var returnValue = result?.Properties["ReturnValue"]?.Value;

            if (returnValue == null) return;

            var mappedValue = Convert.ToUInt32(returnValue); 

            if ( mappedValue != 0)
                throw new InvalidOperationException("Error occured Reason: " + _messages.GetMessage(mappedValue));
        }

        private static void ThrowIfCantFindFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Can't find file in path: " + path);
            }
        }

        private void ThrowIfCantFindService(string configName)
        {
            var service = Get(configName);

            if(service == null)
                throw new ArgumentException("Can't find service with name: "+configName);
        }

        private static string GetStartName(WindowsServiceConfiguration config)
        {
            return config.DriverName ?? 
                String.Join(@"\",config.AccountDomain?? ".", config.AccountName ?? "LocalSystem");

        }

    }
}