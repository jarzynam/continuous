using Continuous.Management;
using Continuous.User.LocalUserGroups;
using Continuous.User.Users;

namespace Continuous.User
{
    public static class FactoryExtension
    {
        public static IUserShell UserShell(this ContinuousManagementFactory factory)
        {
            return new UserShell();
        }

        public static ILocalUserGroupShell LocalUserGroup (this ContinuousManagementFactory factory)
        {
            return new LocalUserGroupShell();
        }
    }
}
