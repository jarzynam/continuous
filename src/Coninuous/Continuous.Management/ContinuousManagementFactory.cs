using Continuous.Management.LocalUserGroups;
using Continuous.Management.Users;

namespace Continuous.Management
{
    public class ContinuousManagementFactory
    {
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
