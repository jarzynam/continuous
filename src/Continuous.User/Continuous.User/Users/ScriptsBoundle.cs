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

    }
}