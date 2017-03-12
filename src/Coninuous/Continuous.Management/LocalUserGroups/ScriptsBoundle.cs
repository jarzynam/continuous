using System;
using System.IO;

namespace Continuous.Management.LocalUserGroups
{
    internal class ScriptsBoundle
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalUserGroup", "Scripts");
        }

        public string CreateLocalUserGroup => Path.Combine(_currentPath, "CreateLocalUserGroup.ps1");
        public string RemoveLocalUserGroup => Path.Combine(_currentPath, "RemoveLocalUserGroup.ps1");
        public string GetLocalUserGroup => Path.Combine(_currentPath, "GetLocalUserGroup.ps1");
        public string AddUsersToLocalGroup => Path.Combine(_currentPath, "AddUsersToLocalGroup.ps1");
        public string RemoveUsersFromLocalGroup => Path.Combine(_currentPath, "RemoveUsersFromLocalGroup.ps1");
    }
}
