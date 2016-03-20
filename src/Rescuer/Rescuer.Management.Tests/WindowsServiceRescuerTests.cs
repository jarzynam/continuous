using System.Collections.Generic;
using System.ServiceProcess;
using Autofac;
using Moq;
using NUnit.Framework;
using Rescuer.Management.Rescuers;
using Rescuer.Management.Rescuers.WindowsService;
using Rescuer.Management.Rescuers.WindowsService.Exceptions;
using Rescuer.Management.Rescuers.WindowsService.Shell;
using Rescuer.Management.Transit;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceRescuerTests
    {
        [Test]
        public void Can_Check_StoppedStatus_WindowsService_Test()
        {
            var builder = new ContainerBuilder();
            var windowsServiceShell = new Mock<IWindowsServiceShell>();
            
            windowsServiceShell.Setup(p => p.GetServiceStatus())
                .Returns(ServiceControllerStatus.Stopped);

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                IRescuer rescuer = container.Resolve<IWindowsServiceRescuer>();            
                var healthStatus = rescuer.CheckHealth();

                Assert.IsNotNull(healthStatus);
                Assert.AreEqual(HealthStatus.Stopped, healthStatus);
            }
        }

        [Test]
        public void Can_Check_WorkingStatus_WindowsService_Test()
        {
            var builder = new ContainerBuilder();
            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.GetServiceStatus())
                .Returns(ServiceControllerStatus.Running);

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                IRescuer rescuer = container.Resolve<IWindowsServiceRescuer>();
                
                var healthStatus = rescuer.CheckHealth();

                Assert.IsNotNull(healthStatus);
                Assert.AreEqual(HealthStatus.Working, healthStatus);
            }
        }

        [Test]
        public void Can_Connect_ToService_Test()
        {
            var builder = new ContainerBuilder();
            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.ConnectToService(It.IsAny<string>()))
                .Returns(true);

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.DoesNotThrow(() => rescuer.Connect("test"));
            }
        }

        [Test]
        public void Can_Handle_Connection_To_NoExistingService_Test()
        {
            var builder = new ContainerBuilder();
            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.ConnectToService(It.IsAny<string>()))
                .Returns(false);
            windowsServiceShell.Setup(p => p.ErrorLog).Returns(new List<string> {"Service connection exception"});

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.Catch<ServiceConnectionException>(() => rescuer.Connect("test"));
            }
        }

        [Test]
        public void Can_Inject_MockedShell_To_WindwosServiceRescuer_Test()
        {
            var builder = new ContainerBuilder();

            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            var shellHits = 0;
            windowsServiceShell.Setup(p => p.GetServiceStatus())
                .Callback(() => { shellHits++; })
                .Returns(ServiceControllerStatus.Running);

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                rescuer.CheckHealth();

                Assert.IsTrue(shellHits > 0, "Windows service rescuer should hit mocked shell at least once.");
            }
        }

        [Test]
        public void Can_Rescue_Service_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(true);

            builder.RegisterInstance(shell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                var result = rescuer.Rescue();

                Assert.IsTrue(result);
            }
        }

        [Test]
        public void Can_Handle_FailedResuce_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(false);
            shell.Setup(p => p.ErrorLog).Returns(new List<string>{"cant start service"});

            builder.RegisterInstance(shell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.Throws<ServiceRescueException>(() => rescuer.Rescue());
            }
        }

        [Test]
        public void Can_Monitor_And_Rescue_RunningService_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus()).Returns(ServiceControllerStatus.Running);

            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();
                Assert.DoesNotThrow(() =>rescuer.MonitorAndRescue());
            }
        }

        [Test]
        public void Can_Monitor_And_Rescue_StoppedService_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus()).Returns(ServiceControllerStatus.Running);
            shell.Setup(p => p.StartService()).Returns(true);

            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();
                Assert.DoesNotThrow(() => rescuer.MonitorAndRescue());
            }
        }
    }
}