using System.Linq;
using Autofac;
using Moq;
using NUnit.Framework;
using Rescuer.Management.WindowsService.Shell;
using Rescuer.Management.WindowsService.Shell.Installer;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceShellTests
    {
        private IContainer _container;

        private string negativeService = "negaticeService";
        private string positiveService = "positiveService";
        [SetUp]
        public void Configure()
        {
            var builder = new ContainerBuilder();


            var installerMock = new Mock<IWindowsServiceInstaller>();
            installerMock.Setup(p => p.Install(positiveService))
                .Returns(true);
            installerMock.Setup(p => p.Uninstall(positiveService))
                .Returns(true);

            installerMock.Setup(p => p.Install(negativeService))
                .Returns(false);
            installerMock.Setup(p => p.Uninstall(negativeService));

            builder.RegisterInstance(installerMock.Object).As<IWindowsServiceInstaller>();
            builder.RegisterType<WindowsServiceShell>().AsImplementedInterfaces();
            
            _container = builder.Build();
        }


        [Test]
        public void Can_Install_Service_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installResult = shell.InstallService(positiveService);

            Assert.AreEqual(true, installResult);
        }

        [Test]
        public void Can_Uninstall_Service_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var uninstallResult = shell.UninstallService(positiveService);

            Assert.AreEqual(true, uninstallResult);
        }
        

        [Test]
        public void Can_Handle_With_InstallError_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            if (shell.ErrorLog.Any())
                shell.ClearErrorLog();

            Assert.IsFalse(shell.ErrorLog.Any(), "error log after cleaning should be empty");

            var installResult = shell.InstallService(negativeService);

            Assert.AreEqual(false, installResult, "installation should fail");
            Assert.IsTrue(shell.ErrorLog.Any(), "after installation failed, error log should contains at least one message ");
            Assert.IsNotNullOrEmpty(shell.ErrorLog[0], "error message should contains any text");
        }

        [Test]
        public void Can_Handle_With_UninstallError_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            if (shell.ErrorLog.Any())
                shell.ClearErrorLog();

            Assert.IsFalse(shell.ErrorLog.Any(), "error log after cleaning should be empty");

            var installResult = shell.UninstallService(negativeService);

            Assert.AreEqual(false, installResult, "uninstallation should fail");
            Assert.IsTrue(shell.ErrorLog.Any(), "after uninstallation failed, error log should contains at least one message ");
            Assert.IsNotNullOrEmpty(shell.ErrorLog[0], "error message should contains any text");
        }

        [Test]
        public void Can_Check_If_ServiceExist()
        {
            Assert.Fail("test not implemented yet");
        }


        [TearDown]
        public void Dispose()
        {
            _container.Dispose();
        }


    }
}
