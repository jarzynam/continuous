using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using Continuous.Management.WindowsService.Shell;
using NUnit.Framework;

namespace Continuous.Management.Tests
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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());
            
            try
            {
                _shell.ConnectToService(serviceName);

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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());
           
            try
            {
                TestDelegate act = () => _shell.ConnectToService(serviceName);

                Assert.DoesNotThrow(act);
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

            TestDelegate act  = () => _shell.ConnectToService(serviceName);

            Assert.Throws<InvalidOperationException>(act, "Service is not connected");
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

            TestDelegate intallDelegate = () =>_shell.InstallService(serviceName, servicePath);

            Assert.DoesNotThrow(intallDelegate);
            
            TestDelegate uninstallDelegate =  () => _shell.UninstallService(serviceName);

            Assert.DoesNotThrow(uninstallDelegate);
        }

        [Test]
        public void Can_Ununinstall_NoExistinx_Service_Test()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                Assert.Fail("user invoking this test must has admiministrator role");

            var serviceName = _helper.RandomServiceName;

            TestDelegate act = () => _shell.UninstallService(serviceName);

            Assert.Throws<System.Management.Automation.RuntimeException>(act, $"{serviceName} not found");

        }

        [Test]
        public void Can_Start_RunningService_Test()
        {
            var serviceName = _helper.RandomServiceName;

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                _shell.ConnectToService(serviceName);

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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());
            
            try
            {
                _shell.ConnectToService(serviceName);
                
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

             _shell.InstallService(serviceName, _helper.GetTestServicePath());
           
            try
            {
                _shell.ConnectToService(serviceName);

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

            _shell.InstallService(serviceName, _helper.GetTestServicePath());
          
            try
            {
                _shell.ConnectToService(serviceName);
                
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

        [Test]
        public void Can_ChangeUser_InExistingServer_Test()
        {
            string userName = "";
            string password = "";
            string serviceName = _helper.RandomServiceName;

            _shell.InstallService(serviceName, _helper.GetTestServicePath());

            try
            {
                _shell.ConnectToService(serviceName);

                _shell.ChangeUser(userName, password);

                _shell.StopService();
                _shell.StartService();

                var service = _shell.GetService();

                Assert.AreEqual(userName, service.UserName);

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
                _shell.ConnectToService(serviceName);

                var service = _shell.GetService();

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
    }
}