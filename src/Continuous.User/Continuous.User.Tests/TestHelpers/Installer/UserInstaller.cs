using Continuous.User.Users;
using Continuous.User.Users.Model;

namespace Continuous.User.Tests.TestHelpers.Installer
{
    public class UserInstaller : Installer
    {
        private readonly IUserShell _shell;

        public UserInstaller()
        {
            _shell = new UserShell();
        }

        public void Install(string userName, string password)
        {
            UserHelper.CreateUser(userName, password);
            AddInstance(userName);
        }

        public void Install(LocalUserCreateModel model)
        {
            _shell.Create(model);
            AddInstance(model.Name);
        }

        private void Remove(string userName)
        {
            UserHelper.DeleteUser(userName);
            RemoveInstance(userName);
        }

        protected override void Uninstall(string instanceName)
        {
            UserHelper.DeleteUser(instanceName);
        }

        public void AddAsInstalled(string userName)
        {
            AddInstance(userName);
        }
    }
}
