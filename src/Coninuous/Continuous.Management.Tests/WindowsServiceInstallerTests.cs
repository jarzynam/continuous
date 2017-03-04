using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rescuer.Management.WindowsService.Shell.Installer;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceInstallerTests
    {

        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            var serviceName = "EmptyTestService";
            var installer = new WindowsServiceInstaller();

            var installResult = installer.Install(serviceName);

            Assert.IsTrue(installResult, $"cant install windows service: {serviceName}");

            var uninstallResult = installer.Uninstall(serviceName);

            Assert.IsTrue(uninstallResult, $"can't uninstall existing service: {serviceName}");
        }
    }
}
