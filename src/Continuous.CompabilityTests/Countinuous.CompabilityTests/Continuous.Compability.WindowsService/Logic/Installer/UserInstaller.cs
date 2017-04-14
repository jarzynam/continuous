using Continuous.Compability.WindowsService.TestHelpers;

namespace Continuous.Compability.WindowsService.Logic.Installer
{
    public class UserInstaller : Installer
    {
        public void Install(string userName, string password)
        {
            UserHelper.CreateUser(userName, password);
            AddInstance(userName);
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
    }
}
