using Continuous.Management.LocalUser;
using Continuous.Management.LocalUserGroup;
using Continuous.Management.WindowsService.Shell;

namespace Continuous.Management
{
    public static class ContinuousManagementFactory
    {
        public static IWindowsServiceShell CreateWindowsServiceShell()
        {
            return new WindowsServiceShell();
        }

        public static ILocalUserShell CreateLocalUserShell()
        {
            return new LocalUserShell();
        }

        public static ILocalUserGroupShell CreateLocalGroupShell()
        {
            return new LocalUserGroupShell();
        }
    }
}
