using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using Autofac;
using Moq;
using NUnit.Framework;
using Rescuer.Management.WindowsService.Shell;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceShellTests
    {
        [SetUp]
        public void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WindowsServiceShell>().AsImplementedInterfaces();

            _container = builder.Build();
        }


        [TearDown]
        public void Dispose()
        {
            _container.Dispose();
        }

        private IContainer _container;

        
        [Test]
        public void Can_Connect_To_InstalledService_Test()
        {
            var serviceName = "TestService";
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(serviceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(serviceName);

                Assert.IsTrue(connectionResult, "Can't connect to properly installed service");
            }
            finally
            {
                shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Check_ServiceStatus_Test()
        {
            var serviceName = "TestService";
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(serviceName, GetTestServicePath());
            if (!installationResult)            
                Assert.Inconclusive("unable to make test due to failed windows service installation");
            try
            {
                var connectionResult = shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = shell.GetServiceStatus();

                Assert.IsNotNullOrEmpty(serviceStatus, "service status shoudn't be empty");

                Assert.IsTrue(serviceStatus == ServiceControllerStatus.Stopped.ToString() || serviceStatus == ServiceControllerStatus.Running.ToString());
            }
            finally
            {
                shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Throw_Exception_If_CheckStatus_Before_ConnectingToService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            Assert.Throws<InvalidOperationException>(() => shell.GetServiceStatus(),
                "invoking GetServiceStatus before ConnectToService() should throw an exception, but didn't");
        }


        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                Assert.Fail("user invoking this test must has admiministrator role");
            }

            var serviceName = "TestService";
            var servicePath = GetTestServicePath();

            var shell = _container.Resolve<IWindowsServiceShell>();

            var installResult = shell.InstallService(serviceName, servicePath);

            Assert.IsTrue(installResult, $"cant install windows service: {serviceName}");

            var uninstallResult = shell.UninstallService(serviceName);

            Assert.IsTrue(uninstallResult, $"can't uninstall existing service: {serviceName}");
        }

        

        private static string GetTestServicePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "CompiledTestService",
                "Rescuer.Services.EmptyTestService.exe");
        }
    }
}