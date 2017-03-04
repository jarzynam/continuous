using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using NUnit.Framework;
using Rescuer.Management.Rescuers.WindowsService.Shell;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceShellTests
    {
        [SetUp]
        public void Configure()
        {
            _shell = new WindowsServiceShell();
            _helper = new CompiledServiceTestHelper();
        }

        [TearDown]
        public void Dispose()
        {
            _shell.Dispose();
        }

        private CompiledServiceTestHelper _helper;
        private WindowsServiceShell _shell;


        [Test]
        public void Can_Check_ServiceStatus_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");
            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = _shell.GetServiceStatus();

                Assert.IsNotNull(serviceStatus, "service status shoudn't be null");

                Assert.IsTrue(serviceStatus == ServiceControllerStatus.Stopped ||
                              serviceStatus == ServiceControllerStatus.Running);
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Connect_To_InstalledService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                Assert.IsTrue(connectionResult, "Can't connect to properly installed service");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Handle_Check_ServiceStatus_Before_InstallService_Test()
        {
            Assert.Throws<InvalidOperationException>(() => _shell.GetServiceStatus());
        }

        [Test]
        public void Can_Handle_Invalid_ServiceName_InConnection_Test()
        {
            var serviceName = "ReallyBadServiceName12323432";

            if (_shell.ErrorLog.Any())
                Assert.Inconclusive("ErrorLog should be empty before connection attempt");

            var connectionResult = _shell.ConnectToService(serviceName);

            Assert.IsFalse(connectionResult, "connectionResult should be false");
            Assert.IsTrue(_shell.ErrorLog.Any(), "Error log should contains any message after connection attempt");
        }

        [Test]
        public void Can_Handle_StartingService_Before_ConnectToService_Test()
        {
            Assert.Throws<InvalidOperationException>(() => _shell.StartService());
        }

        [Test]
        public void Can_Handle_StoppingService_Before_ConnectToService_Test()
        {
            Assert.Throws<InvalidOperationException>(() => _shell.StopService());
        }


        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                Assert.Fail("user invoking this test must has admiministrator role");

            var serviceName = _helper.RandomServiceName;
            var servicePath = _helper.GetTestServicePath();

            var installResult = _shell.InstallService(serviceName, servicePath);

            Assert.IsTrue(installResult, $"cant install windows service: {serviceName}");

            var uninstallResult = _shell.UninstallService(serviceName);

            Assert.IsTrue(uninstallResult, $"can't uninstall existing service: {serviceName}");
        }

        [Test]
        public void Can_Start_RunningService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var startResult = _shell.StartService();
                if (!startResult)
                    Assert.Inconclusive("unable to make test due to falied start service");

                startResult = _shell.StartService();
                Assert.IsFalse(startResult, "starting running service should return false");

                var serviceStatus = _shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Running, serviceStatus,
                    $"service should be still running but was {serviceStatus}");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Start_StoppedService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = _shell.GetServiceStatus();
                if (ServiceControllerStatus.Stopped != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be runnig but was {serviceStatus})");

                var startResult = _shell.StartService();
                Assert.IsTrue(startResult, "start result should be true");

                serviceStatus = _shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Running, serviceStatus,
                    $"after positive start result, service status should be 'running', but was {serviceStatus}");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Stop_RunningService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var startResult = _shell.StartService();

                if (!startResult)
                    Assert.Inconclusive("unable to make test due to failed windows service start");

                var stopResult = _shell.StopService();

                Assert.IsTrue(stopResult, "can't get positivie result from StopService function");

                var serviceStatus = _shell.GetServiceStatus();

                Assert.AreEqual(ServiceControllerStatus.Stopped, serviceStatus,
                    $"after positive stop result, service status should be also as 'stopped' but was {serviceStatus}");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Stop_StoppedService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            var installationResult = _shell.InstallService(serviceName, _helper.GetTestServicePath());
            if (!installationResult)
                Assert.Inconclusive("unable to make test due to failed windows service installation");

            try
            {
                var connectionResult = _shell.ConnectToService(serviceName);

                if (!connectionResult)
                    Assert.Inconclusive("unable to make test due to failed connect to service");

                var serviceStatus = _shell.GetServiceStatus();
                if (ServiceControllerStatus.Stopped != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be stopped but was {serviceStatus}");

                var stopResult = _shell.StopService();
                Assert.IsFalse(stopResult, "stopping stopped service should return false");

                serviceStatus = _shell.GetServiceStatus();
                Assert.AreEqual(ServiceControllerStatus.Stopped, serviceStatus,
                    $"service should be still stopped but was {serviceStatus}");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }

        [Test]
        public void Can_Throw_Exception_If_CheckStatus_Before_ConnectingToService_Test()
        {
            Assert.Throws<InvalidOperationException>(() => _shell.GetServiceStatus(),
                "invoking GetServiceStatus before Connect() should throw an exception, but didn't");
        }
    }
}