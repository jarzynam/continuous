using System.Collections.Generic;
using Continuous.WindowsService.Model;

namespace Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo
{
    internal interface IWindowsServiceModelManager
    {
        void CopyProperties(Model.WindowsServiceInfo target, Model.WindowsServiceInfo source);
        WindowsServiceConfigurationForUpdate CreateBackupConfig(Model.WindowsServiceInfo originalService, ConfigurationCache cachedChanges);
    }

    internal class WindowsServiceModelManager : IWindowsServiceModelManager
    {
        public void CopyProperties(Model.WindowsServiceInfo target,  Model.WindowsServiceInfo source)
        {
            target.AccountDomain = source.AccountDomain;
            target.AccountName = source.AccountName;
            target.CanPause = source.CanPause;
            target.CanStop = source.CanStop;
            target.Description = source.Description;
            target.DisplayName = source.DisplayName;
            target.ErrorControl = source.ErrorControl;
            target.ExitCode = source.ExitCode;
            target.InteractWithDesktop = source.InteractWithDesktop;
            target.Path = source.Path;
            target.ProcessId = source.ProcessId;
            target.ServiceDependencies = new List<string>(source.ServiceDependencies);
            target.ServiceSpecificExitCode = source.ServiceSpecificExitCode;
            target.State = source.State;
            target.Status = source.Status;
            target.StartMode = source.StartMode;
        }

        public WindowsServiceConfigurationForUpdate CreateBackupConfig(Model.WindowsServiceInfo originalService, ConfigurationCache cachedChanges)
        {
            var config = new WindowsServiceConfigurationForUpdate();

            if (cachedChanges.Description != null) config.Description = originalService.Description;
            if (cachedChanges.DisplayName != null) config.DisplayName = originalService.DisplayName;
            if (cachedChanges.Path != null) config.Path = originalService.Path;
            if (cachedChanges.ServiceDependencies != null) config.ServiceDependencies = originalService.ServiceDependencies;
            if (cachedChanges.ErrorControl != null) config.ErrorControl = originalService.ErrorControl;
            if (cachedChanges.InteractWithDesktop != null) config.InteractWithDesktop = originalService.InteractWithDesktop;
            if (cachedChanges.StartMode != null) config.StartMode = originalService.StartMode;
            if (cachedChanges.Type != null) config.Type = originalService.Type;

            return config;
        }
    }
}