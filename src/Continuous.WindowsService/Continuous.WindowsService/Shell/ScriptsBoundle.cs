using Continuous.Management.Common;

namespace Continuous.WindowsService.Shell
{
    internal class ScriptsBoundle : BoundleBase
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = AddToPath(BasePath, "Scripts");
        }

        internal string UninstallService => AddToPath(_currentPath, "UninstallService.ps1");
        internal string InstallService => AddToPath(_currentPath, "InstallService.ps1");
        internal string ChangeAccount => AddToPath(_currentPath, "ChangeAccount.ps1");
        internal string GetService => AddToPath(_currentPath, "GetService.ps1");
        internal string InstallServiceWithParameters => AddToPath(_currentPath, "InstallServiceWithParameters.ps1");
        internal string UpdateServiceWithParameters => AddToPath(_currentPath, "UpdateServiceWithParameters.ps1");
        internal string GetAllServices => AddToPath(_currentPath, "GetAllServices.ps1");
        internal string PauseService => AddToPath(_currentPath, "PauseService.ps1");
        internal string ResumeService => AddToPath(_currentPath, "ResumeService.ps1");
        internal string StartService => AddToPath(_currentPath, "StartService.ps1");
        internal string StopService => AddToPath(_currentPath, "StopService.ps1");
        internal string GetServiceState => AddToPath(_currentPath, "GetServiceState.ps1");
        internal string ExistsService => AddToPath(_currentPath, "ExistsService.ps1");
        internal string UpdateServiceRegistryProperty => AddToPath(_currentPath, "UpdateServiceRegistryProperty.ps1");
        internal string ExecuteCommand => AddToPath(_currentPath, "ExecuteCommand.ps1");
    }
}
