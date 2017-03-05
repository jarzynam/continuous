using Continuous.Management.Common;
using Continuous.Management.WindowsService.Shell;

namespace Continuous.Management.LocalUser
{
    public class UserShell
    {
        private ScriptExecutor _executor;
        private ScriptsBoundle _scriptsPath;

        public UserShell()
        {
            _executor = new ScriptExecutor();
            _scriptsPath = new ScriptsBoundle();
        }

        public void AddUser()
        {
            
        }

        public void DeleteUser()
        {
            
        }

        public void GetUsers()
        {
            
        }

    }
}
