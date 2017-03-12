using Continuous.Management.LocalUserGroups;
using Continuous.Management.Users;
using Continuous.Management.WindowsServices.Shell;

namespace Continuous.Management
{
    public static class ContinuousManagementFactory
    {
        public static IWindowsServiceShell CreateWindowsServiceShell()
        {
            return new WindowsServiceShell();
        }

        public static IUserShell CreateLocalUserShell()
        {
            return new UserShell();
        }

        public static ILocalUserGroupShell CreateLocalGroupShell()
        {
            return new LocalUserGroupShell();
        }
    }
}
