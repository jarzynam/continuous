using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using Continuous.Management.Common;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Resources;
using Microsoft.Win32;

namespace Continuous.WindowsService.Shell
{

    /// <summary>
    /// Windows Service Shell implementation
    /// </summary>
    public class WindowsServiceShell : IWindowsServiceShell
    {
        private readonly IScriptExecutor _executor;
        private readonly IWindowsServiceExceptionFactory _exceptionFactoryFactory;
        private readonly ScriptsBoundle _scripts;

        private const int DelayTimeInMiliseconds = 250;
        private readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(1000);

        private readonly Mapper _mapper;


        /// <summary>
        /// Windows service shell
        /// </summary>
        public WindowsServiceShell()
        {

            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();

            _exceptionFactoryFactory = new WindowsServiceExceptionFactory(new Win32ServiceMessages());
            _mapper = new Mapper();
        }

        /// <inheritdoc />
        public WindowsServiceState GetState(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.GetServiceState, parameters);

            ThrowServiceExceptionIfNecessary(result);

            return _mapper.MapServiceState(result.FirstOrDefault());
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Install(WindowsServiceConfiguration config)
        {
            ThrowIfCantFindFile(config.Path);

            var startName = GetStartName(config);

            var isDelayedStart = config.StartMode == WindowsServiceStartMode.AutomaticDelayedStart;
            var startMode = isDelayedStart ? WindowsServiceStartMode.Automatic: config.StartMode;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", config.Name),
                new CommandParameter("displayName", config.DisplayName??config.Name),
                new CommandParameter("errorControl", (byte?) config.ErrorControl),
                new CommandParameter("startMode",  startMode),
                new CommandParameter("serviceType", (byte?) config.Type),
                new CommandParameter("desktopInteract", config.InteractWithDesktop),
                new CommandParameter("fullServicePath", config.Path),
                new CommandParameter("startPassword", config.AccountPassword),
                new CommandParameter("startName", startName),
                new CommandParameter("serviceDependencies", config.ServiceDependencies?.Count > 0 ? config.ServiceDependencies : null)
            };

            var result = _executor.Execute(_scripts.InstallServiceWithParameters, parameters);

            if(config.Description != null)
                UpdateDescription(config.Name, config.Description);

            if(isDelayedStart)
                UpdateDelayedStart(config.Name, true);

            ThrowServiceExceptionIfNecessary(result);
        }


        /// <inheritdoc />
        public void Update(string serviceName, WindowsServiceConfigurationForUpdate config)
        {
            ThrowIfCantFindService(serviceName);

            if(config.Path != null)
                ThrowIfCantFindFile(config.Path);

            var isDelayedStart = config.StartMode == WindowsServiceStartMode.AutomaticDelayedStart;
            var startMode = isDelayedStart ? WindowsServiceStartMode.Automatic : config.StartMode;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("displayName", config.DisplayName),
                new CommandParameter("errorControl", (byte?) config.ErrorControl),
                new CommandParameter("startMode",   startMode),
                new CommandParameter("serviceType", (byte?) config.Type),
                new CommandParameter("desktopInteract", config.InteractWithDesktop.GetValueOrDefault()),
                new CommandParameter("fullServicePath", config.Path),
                new CommandParameter("serviceDependencies", config.ServiceDependencies?.Count == 0 ? new [] {String.Empty} :  config.ServiceDependencies)
            };

            var result = _executor.Execute(_scripts.UpdateServiceWithParameters, parameters);

            if (config.Description != null)
                UpdateDescription(serviceName, config.Description);

            UpdateDelayedStart(serviceName, isDelayedStart);

            ThrowServiceExceptionIfNecessary(result);
        }

        /// <inheritdoc />
        public void Uninstall(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter(nameof(serviceName), serviceName)
            };

            var result = _executor.Execute(_scripts.UninstallService, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        /// <inheritdoc />
        public bool Stop(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var state = GetState(serviceName);

            if ( state != WindowsServiceState.Running && state != WindowsServiceState.Paused)
                return false;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.StopService, parameters);

            ThrowServiceExceptionIfNecessary(result);

            return true;
        }

        /// <inheritdoc />
        public bool Start(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var status = GetState(serviceName);

            if (status != WindowsServiceState.Stopped)
                return false;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.StartService, parameters);

            ThrowServiceExceptionIfNecessary(result);

            return true;
        }

        /// <inheritdoc />
        public bool Continue(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var status = GetState(serviceName);

            if (status != WindowsServiceState.Paused)
                return false;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.ResumeService, parameters);

            ThrowServiceExceptionIfNecessary(result);

            return true;
        }

        /// <inheritdoc />
        public bool Pause(string serviceName)
        {
            ThrowIfCantFindService(serviceName);

            var status = GetState(serviceName);

            if (status != WindowsServiceState.Running)
                return false;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.PauseService, parameters);

            ThrowServiceExceptionIfNecessary(result);

            return true;
        }

        /// <inheritdoc />
        public void WaitForState(string serviceName, WindowsServiceState state, TimeSpan timeout)
        {
            ThrowIfCantFindService(serviceName);

            var startedTime = DateTime.UtcNow;

            do
            {
                if (GetState(serviceName) == state)
                    return;

                Thread.Sleep(DelayTimeInMiliseconds);

            } while (DateTime.UtcNow - startedTime < timeout);

            throw new TimeoutException();
        }

        /// <inheritdoc />
        public void WaitForState(string serviceName, WindowsServiceState state)
        {
            WaitForState(serviceName, state, _timeout);
        }

        /// <inheritdoc />
        public bool Exists(string serviceName)
        {
            var parameters = new List<CommandParameter>()
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.ExistsService, parameters);

            return result.Select(p => (bool) p.BaseObject).FirstOrDefault();
        }

        /// <inheritdoc />
        public void ExecuteCommand(string serviceName, int commandCode)
        {
            ThrowIfCantFindService(serviceName);

            if(commandCode < 128 || commandCode > 256)
                throw new ArgumentException("Command code value must be between 128 and 256, inclusive but was: " + commandCode);

            var parameters = new List<CommandParameter>()
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("commandCode", commandCode)
            };

            var result = _executor.Execute(_scripts.ExecuteCommand, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        /// <inheritdoc />
        public WindowsServiceInfo Get(string serviceName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName)
            };

            var result = _executor.Execute(_scripts.GetService, parameters).FirstOrDefault();

            return result == null ? null : _mapper.Map(result);
        }

        /// <inheritdoc />
        public List<WindowsServiceInfo> GetAll()
        {
            var parameters = new List<CommandParameter>();

            var result = _executor.Execute(_scripts.GetAllServices, parameters);

            return result.Select(p => _mapper.Map(p)).ToList();
        }

        /// <inheritdoc />
        public void ChangeAccount(string serviceName, string accountName, string password, string domain = ".")
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("newAccount", string.Join(@"\", domain??".", accountName)),
                new CommandParameter("newPassword", password)
            };

            var result = _executor.Execute(_scripts.ChangeAccount, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        private void UpdateDescription(string serviceName, string description)
        {
            UpdateRegistryProperty(serviceName, "Description", description, "ExpandString");
        }

        private void UpdateDelayedStart(string serviceName, bool isDelayedStart)
        {
           var isDelayedStr = Convert.ToInt32(isDelayedStart).ToString();
           UpdateRegistryProperty(serviceName, "DelayedAutostart", isDelayedStr, RegistryValueKind.DWord.ToString() );
        }

        private void UpdateRegistryProperty(string serviceName, string propertyName, string propertyValue, string propertyType)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("serviceName", serviceName),
                new CommandParameter("propertyName", propertyName),
                new CommandParameter("propertyValue", propertyValue),
                new CommandParameter("propertyType", propertyType)

            };

            var result = _executor.Execute(_scripts.UpdateServiceRegistryProperty, parameters);

            ThrowServiceExceptionIfNecessary(result);
        }

        private void ThrowServiceExceptionIfNecessary(ICollection<PSObject> results)
        {
            var result = results.FirstOrDefault();

            var returnValue = result?.Properties["ReturnValue"]?.Value;

            if (returnValue == null) return;

            var mappedValue = Convert.ToUInt32(returnValue); 

            if(mappedValue != 0)
                _exceptionFactoryFactory.ServiceException(mappedValue);
        }

        private void ThrowIfCantFindFile(string path)
        {
            if (!File.Exists(path))
                _exceptionFactoryFactory.FileNotFoundException(path);
        }

        private void ThrowIfCantFindService(string serviceName)
        {
            if(!Exists(serviceName))
                _exceptionFactoryFactory.ServiceNotFoundException(serviceName);
        }

        private static string GetStartName(WindowsServiceConfiguration config)
        {
            return config.DriverName ?? 
                String.Join(@"\",config.AccountDomain?? ".", config.AccountName ?? "LocalSystem");
        }
    }
}