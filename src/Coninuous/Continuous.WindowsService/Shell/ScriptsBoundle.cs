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
        internal string ChangeUser => AddToPath(_currentPath, "ChangeUser.ps1");
        internal string GetService => AddToPath(_currentPath, "GetService.ps1");
        internal string InstallServiceWithParameters => AddToPath(_currentPath, "InstallServiceWithParameters.ps1");
        internal string UpdateServiceWithParameters => AddToPath(_currentPath, "UpdateServiceWithParameters.ps1");
        internal string GetAllServices => AddToPath(_currentPath, "GetAllServices.ps1");
    }
}
