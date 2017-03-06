using System;
using System.IO;

namespace Continuous.Management.LocalUserGroup
{
    public class ScriptsBoundle
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalLocalUserGroup", "Scripts");
        }

        public string CreateLocalUserGroup => Path.Combine(_currentPath, "CreateLocalUserGroup.ps1");
        public string RemoveLocalUserGroup => Path.Combine(_currentPath, "RemoveLocalUserGroup.ps1");
        public string GetLocalUserGroup => Path.Combine(_currentPath, "GetLocalUserGroup.ps1");
        public string AddUSerToLocalGroup => Path.Combine(_currentPath, "AddUserToLocalGroup.ps1");
        public string RemoveUserFromLocalGroup => Path.Combine(_currentPath, "RemoveUserFromLocalGroup.ps1");
    }
}
