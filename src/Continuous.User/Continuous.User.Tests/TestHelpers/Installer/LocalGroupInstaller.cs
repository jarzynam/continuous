using System;
using Continuous.User.LocalUserGroups;

namespace Continuous.User.Tests.TestHelpers.Installer
{
    public class LocalGroupInstaller : Installer
    {
        private readonly ILocalUserGroupShell _shell;
        public string Description { get; } = "test group to delete";

        public LocalGroupInstaller()
        {
            _shell = new LocalUserGroupShell();
        }

        public void Install(string name)
        {
            _shell.Create(name, Description);
            AddInstance(name);
        }

        public void AddAsInstalled(string name)
        {
            AddInstance(name);
        }

        protected override void Uninstall(string instanceName)
        {
            try
            {
                _shell.Remove(instanceName);
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }
}
