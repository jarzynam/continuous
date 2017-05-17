using Continuous.WindowsService.Model;

namespace Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo
{
    internal class ConfigurationCache : WindowsServiceConfigurationForUpdate
    {
        internal string AccountName { get; set; }
        internal string AccountPassword { get; set; }
        internal string AccountDomain { get; set; }

        internal bool RollbackOnError { get; set; }

        internal void Clear()
        {
            AccountName = null;
            AccountPassword = null;
            AccountDomain = null;

            Description = null;
            DisplayName = null;
            ErrorControl = null;
            InteractWithDesktop = null;
            Path = null;
            ServiceDependencies = null;
            StartMode = null;
            Type = null;
            
            RollbackOnError = false;
        }

        internal bool HasUserChanged()
        {
            return AccountName != null ||
                   AccountPassword != null ||
                   AccountDomain != null;
        }



    }
}