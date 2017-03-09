using System;
using System.Management.Automation;
using System.Security.Principal;
using System.ServiceProcess;
using Continuous.Management.LocalUser;
using Continuous.Management.WindowsService.Shell;
using NUnit.Framework;

namespace Continuous.Management.Tests.WindowsService
{
    [TestFixture]
    public class WindowsServiceShellTests
    {
        [SetUp]
        public void Configure()
        {
            _shell = new WindowsServiceShell();
            _userShell = new LocalUserShell();

            _helper = new CompiledServiceTestHelper();
        }

        private CompiledServiceTestHelper _helper;
        private IWindowsServiceShell _shell;
        private ILocalUserShell _userShell;

        [Test]
        public void Can_ChangeUser_InExistingServer_Test()
        {
            var user = new Management.LocalUser.Model.LocalUser {Name = _helper.RandomServiceName + "User"};
            _userShell.CreateUser(user);

            var serviceName = _helper.RandomServiceName;
            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                _shell.ChangeUser(serviceName, user.Name, user.Password);

                var service = _shell.GetService(serviceName);

                Assert.AreEqual($".\\{user.Name}", service.UserName);
            }
            finally
            {
                _shell.UninstallService(serviceName);
                _userShell.RemoveUser(user.Name);
            }
        }


        [Test]
        public void Can_Check_ServiceStatus_Test()
        {
            var serviceName = _helper.RandomServiceName;

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var serviceStatus = _shell.GetServiceStatus(serviceName);

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
        public void Can_GetService_WhenExists_Test()
        {
            var serviceName = _helper.RandomServiceName;
            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var service = _shell.GetService(serviceName);

                Assert.AreEqual(service.Name, serviceName);
                Assert.AreEqual(service.Description, null);
                Assert.AreEqual(service.DisplayName, serviceName);
                Assert.AreEqual(service.ProcessId, 0);
                Assert.AreEqual(service.UserName, "LocalSystem");
            }
            finally
            {
                _shell.UninstallService(serviceName);
            }
        }


        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                Assert.Fail("user invoking this test must has admiministrator role");

            var serviceName = _helper.RandomServiceName;
            var servicePath = _helper.GetTestServicePath();

            TestDelegate intallDelegate = () => _shell.InstallService(serviceName, servicePath);

            Assert.DoesNotThrow(intallDelegate);

            TestDelegate uninstallDelegate = () => _shell.UninstallService(serviceName);

            Assert.DoesNotThrow(uninstallDelegate);
        }

        [Test]
        public void Can_Start_RunningService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var startResult = _shell.StartService(serviceName);
                if (!startResult)
                    Assert.Inconclusive("unable to make test due to falied start service");

                startResult = _shell.StartService(serviceName);
                Assert.IsFalse(startResult, "starting running service should return false");

                var serviceStatus = _shell.GetServiceStatus(serviceName);
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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var serviceStatus = _shell.GetServiceStatus(serviceName);
                if (ServiceControllerStatus.Stopped != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be runnig but was {serviceStatus})");

                var startResult = _shell.StartService(serviceName);
                Assert.IsTrue(startResult, "start result should be true");

                serviceStatus = _shell.GetServiceStatus(serviceName);
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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var startResult = _shell.StartService(serviceName);

                if (!startResult)
                    Assert.Inconclusive("unable to make test due to failed windows service start");

                var stopResult = _shell.StopService(serviceName);

                Assert.IsTrue(stopResult, "can't get positivie result from StopService function");

                var serviceStatus = _shell.GetServiceStatus(serviceName);

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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                var serviceStatus = _shell.GetServiceStatus(serviceName);
                if (ServiceControllerStatus.Stopped != serviceStatus)
                    Assert.Inconclusive(
                        $"unable to make test due to incorrect windows service status (should be stopped but was {serviceStatus}");

                var stopResult = _shell.StopService(serviceName);
                Assert.IsFalse(stopResult, "stopping stopped service should return false");

                serviceStatus = _shell.GetServiceStatus(serviceName);
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
            Assert.Throws<InvalidOperationException>(() => _shell.GetServiceStatus("TEST213214"),
                "invoking GetServiceStatus before Connect() should throw an exception, but didn't");
        }

        [Test]
        public void Can_Ununinstall_NoExistinx_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                Assert.Fail("user invoking this test must has admiministrator role");

            var serviceName = _helper.RandomServiceName;

            TestDelegate act = () => _shell.UninstallService(serviceName);

            Assert.Throws<RuntimeException>(act, $"{serviceName} not found");
        }
    }
}