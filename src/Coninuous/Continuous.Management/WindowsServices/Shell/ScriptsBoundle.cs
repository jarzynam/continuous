using System;
using System.IO;

namespace Continuous.Management.WindowsServices.Shell
{
    internal class ScriptsBoundle
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsServices", "Scripts");
        }

        internal string UninstallService => Path.Combine(_currentPath, "UninstallService.ps1");
        internal string InstallService => Path.Combine(_currentPath, "InstallService.ps1");
        internal string ChangeUser => Path.Combine(_currentPath, "ChangeUser.ps1");
        internal string GetService => Path.Combine(_currentPath, "GetService.ps1");
        internal string InstallServiceWithParameters => Path.Combine(_currentPath, "InstallServiceWithParameters.ps1");
    }
}
