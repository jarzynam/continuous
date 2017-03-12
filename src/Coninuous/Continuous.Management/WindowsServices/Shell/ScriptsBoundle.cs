using System;
using System.IO;

namespace Continuous.Management.WindowsServices.Shell
{
    internal class ScriptsBoundle
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService", "Scripts");
        }

        public string UninstallService => Path.Combine(_currentPath, "UninstallService.ps1");
        public string InstallService => Path.Combine(_currentPath, "InstallService.ps1");
        public string ChangeUser => Path.Combine(_currentPath, "ChangeUser.ps1");
        public string GetService => Path.Combine(_currentPath, "GetService.ps1");
    }
}
