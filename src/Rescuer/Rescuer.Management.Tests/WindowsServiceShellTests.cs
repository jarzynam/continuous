using System;
using System.IO;
using System.Security.Principal;
using System.ServiceProcess;
using Autofac;
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

            //if for some reasongs, TestService is still running after last tests - try to uninstall it
            new WindowsServiceShell().UninstallService(ServiceName);
        }


        [TearDown]
        public void Dispose()
        {
            _container.Dispose();
        }

        private IContainer _container;

        private static string GetTestServicePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "CompiledTestService",
                "Rescuer.Services.EmptyTestService.exe");
        }

        private const string ServiceName = "TestService23442";

        [Test]
        public void Can_Check_ServiceStatus_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(ServiceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");
            try
            {
                var connectionResult = shell.ConnectToService(ServiceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = shell.GetServiceStatus();

                Assert.IsNotNullOrEmpty(serviceStatus, "service status shoudn't be empty");

                Assert.IsTrue(serviceStatus == ServiceControllerStatus.Stopped.ToString() ||
                              serviceStatus == ServiceControllerStatus.Running.ToString());
            }
            finally
            {
                shell.UninstallService(ServiceName);
            }
        }


        [Test]
        public void Can_Connect_To_InstalledService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(ServiceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(ServiceName);

                Assert.IsTrue(connectionResult, "Can't connect to properly installed service");
            }
            finally
            {
                shell.UninstallService(ServiceName);
            }
        }

        [Test]
        public void Can_Handle_StartingService_Before_ConnectToService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            Assert.Throws<InvalidOperationException>(() => shell.StartService());
        }

        [Test]
        public void Can_Handle_StoppingService_Before_ConnectToService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            Assert.Throws<InvalidOperationException>(() => shell.StopService());
        }


        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                Assert.Fail("user invoking this test must has admiministrator role");
            }


            var servicePath = GetTestServicePath();

            var shell = _container.Resolve<IWindowsServiceShell>();

            var installResult = shell.InstallService(ServiceName, servicePath);

            Assert.IsTrue(installResult, $"cant install windows service: {ServiceName}");

            var uninstallResult = shell.UninstallService(ServiceName);

            Assert.IsTrue(uninstallResult, $"can't uninstall existing service: {ServiceName}");
        }

        [Test]
        public void Can_Start_RunningService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();
            var serviceName = "TestService";

            var installationResult = shell.InstallService(serviceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var startResult = shell.StartService();
                if (!startResult)
                    Assert.Inconclusive("unable to make test due to falied start service");

                startResult = shell.StartService();
                Assert.IsFalse(startResult, "starting running service should return false");

                var serviceStatus = shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Running.ToString(), serviceStatus,
                    $"service should be still running but was {serviceStatus}");
            }
            finally
            {
                shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Start_StoppedService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(ServiceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(ServiceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = shell.GetServiceStatus();
                if (ServiceControllerStatus.Stopped.ToString() != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be runnig but was {serviceStatus})");

                var startResult = shell.StartService();
                Assert.IsTrue(startResult, "start result should be true");

                serviceStatus = shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Running.ToString(), serviceStatus,
                    $"after positive start result, service status should be 'running', but was {serviceStatus}");
            }
            finally
            {
                shell.UninstallService(ServiceName);
            }
        }

        [Test]
        public void Can_Stop_RunningService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();


            var installationResult = shell.InstallService(ServiceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(ServiceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var startResult = shell.StartService();

                if (!startResult)
                    Assert.Inconclusive("unable to make test due to failed windows service start");

                var stopResult = shell.StopService();

                Assert.IsTrue(stopResult, "can't get positivie result from StopService function");

                var serviceStatus = shell.GetServiceStatus();

                Assert.AreEqual(ServiceControllerStatus.Stopped.ToString(), serviceStatus,
                    $"after positive stop result, service status should be also as 'stopped' but was {serviceStatus}");
            }
            finally
            {
                shell.UninstallService(ServiceName);
            }
        }

        [Test]
        public void Can_Stop_StoppedService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            var installationResult = shell.InstallService(ServiceName, GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = shell.ConnectToService(ServiceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = shell.GetServiceStatus();
                if (ServiceControllerStatus.Stopped.ToString() != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be stopped but was {serviceStatus}");

                var stopResult = shell.StopService();
                Assert.IsFalse(stopResult, "stopping stopped service should return false");

                serviceStatus = shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Stopped.ToString(), serviceStatus,
                    $"service should be still stopped but was {serviceStatus}");
            }
            finally
            {
                shell.UninstallService(ServiceName);
            }
        }

        [Test]
        public void Can_Throw_Exception_If_CheckStatus_Before_ConnectingToService_Test()
        {
            var shell = _container.Resolve<IWindowsServiceShell>();

            Assert.Throws<InvalidOperationException>(() => shell.GetServiceStatus(),
                "invoking GetServiceStatus before ConnectToService() should throw an exception, but didn't");
        }
    }
}