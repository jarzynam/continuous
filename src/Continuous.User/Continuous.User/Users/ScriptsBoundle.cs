using Continuous.Management.Common;

namespace Continuous.User.Users
{
    internal class ScriptsBoundle : BoundleBase
    {
        private readonly string _currentPath;

        public ScriptsBoundle()
        {
            _currentPath = AddToPath(BasePath, "Users", "Scripts");
        }

        public string CreateUser => AddToPath(_currentPath, "CreateUser.ps1");
        public string RemoveUser => AddToPath(_currentPath, "RemoveUser.ps1");
        public string GetUser => AddToPath(_currentPath, "GetUser.ps1");
        public string ChangeUserPassword => AddToPath(_currentPath, "ChangeUserPassword.ps1");
        public string ExistsUser => AddToPath(_currentPath, "ExistsUser.ps1");
        public string SetUserFlag => AddToPath(_currentPath, "SetUserFlag.ps1");
        public string SetUserProperty => AddToPath(_currentPath, "SetUserProperty.ps1");
        public string SetUserPropertyDate => AddToPath(_currentPath, "SetUserPropertyDate.ps1");
        public string GetLoggedUsername => AddToPath(_currentPath, "GetLoggedUsername.ps1");
        public string GetAllUsers => AddToPath(_currentPath, "GetAllUsers.ps1");
        public string SetUserVisibility => AddToPath(_currentPath, "SetUserVisibility.ps1");
        public string GetUserVisibility => AddToPath(_currentPath, "GetUserVisibility.ps1");
    }
}