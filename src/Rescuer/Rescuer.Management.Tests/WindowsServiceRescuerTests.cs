using System.ServiceProcess;
using Autofac;
using Moq;
using NUnit.Framework;
using Rescuer.Management.WindowsService;
using Rescuer.Management.WindowsService.Shell;

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

                rescuer.ConnectToService("RescuerTestService");
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

                rescuer.ConnectToService("RescuerTestService");
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

                Assert.DoesNotThrow(() => rescuer.ConnectToService("test"));
            }
        }

        [Test]
        public void Can_Handle_Connection_To_NoExistingService_Test()
        {
            var builder = new ContainerBuilder();
            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.ConnectToService(It.IsAny<string>()))
                .Returns(false);

            builder.RegisterInstance(windowsServiceShell.Object).As<IWindowsServiceShell>();
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.Catch<ServiceNotFoundException>(() => rescuer.ConnectToService("test"));
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

                rescuer.ConnectToService("RescuerTestService");
                rescuer.CheckHealth();

                Assert.IsTrue(shellHits > 0, "Windows service rescuer should hit mocked shell at least once.");
            }
        }
    }
}