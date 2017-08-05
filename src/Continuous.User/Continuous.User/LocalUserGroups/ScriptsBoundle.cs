using Continuous.Management.Common;

namespace Continuous.User.LocalUserGroups
{
    internal class ScriptsBoundle : BoundleBase
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = AddToPath(BasePath, "LocalUserGroups", "Scripts");
        }

        public string CreateLocalUserGroup => AddToPath(_currentPath, "CreateLocalUserGroup.ps1");
        public string RemoveLocalUserGroup => AddToPath(_currentPath, "RemoveLocalUserGroup.ps1");
        public string GetLocalUserGroup => AddToPath(_currentPath, "GetLocalUserGroup.ps1");
        public string AddUsersToLocalGroup => AddToPath(_currentPath, "AddUsersToLocalGroup.ps1");
        public string RemoveUsersFromLocalGroup => AddToPath(_currentPath, "RemoveUsersFromLocalGroup.ps1");
        public string GetLocalUserGroupBySid => AddToPath(_currentPath, "GetLocalUserGroupBySid.ps1");
    }
}
