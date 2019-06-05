using System.Collections.Concurrent;
using System.Security.Principal;
using Continuous.User.Users;
using Continuous.User.Users.Model;

namespace Continuous.User.Tests.TestHelpers.Installer
{
    public class UserInstaller : Installer
    {
        private readonly IUserShell _shell;

        private readonly ConcurrentDictionary<string, SecurityIdentifier> _userSecurityIdByName;

        public UserInstaller()
        {
            _shell = new UserShell();
            _userSecurityIdByName = new ConcurrentDictionary<string, SecurityIdentifier>();
        }

        public void Install(string userName, string password)
        {
            UserHelper.CreateUser(userName, password);
            AddInstance(userName);
        }

        public void InstallWithProfile(string userName, string password)
        {
            Install(userName, password);

            var userSecurityId = UserHelper.CreateUserProfile(userName);

            _userSecurityIdByName.AddOrUpdate(userName, userSecurityId, (key, value) => value);
        }

        public void Install(LocalUserCreateModel model)
        {
            _shell.Create(model);
            AddInstance(model.Name);
        }

        public void Remove(string userName)
        {
            UserHelper.DeleteUser(userName);
            RemoveInstance(userName);
        }

        protected override void Uninstall(string instanceName)
        {
            if (_userSecurityIdByName.TryRemove(instanceName, out var id))
            {
                UserHelper.DeleteUserProfile(id);
            }

            UserHelper.DeleteUser(instanceName);
        }

        public void AddAsInstalled(string userName)
        {
            AddInstance(userName);
        }
    }
}
