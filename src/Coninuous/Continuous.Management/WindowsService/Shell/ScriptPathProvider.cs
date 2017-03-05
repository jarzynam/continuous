using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rescuer.Management.WindowsService.Shell
{
    public class ScriptPathProvider
    {
        private readonly string _currentPath;
        
        public ScriptPathProvider()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WindowsService", "Scripts");
        }

        public string UninstallService => Path.Combine(_currentPath, "UninstallService.ps1");
        public string InstallService => Path.Combine(_currentPath, "InstallService.ps1");
    }
}
