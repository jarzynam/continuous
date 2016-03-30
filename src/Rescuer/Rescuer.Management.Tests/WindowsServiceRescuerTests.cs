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

                Assert.DoesNotThrow( () => rescuer.Rescue());                
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

                var rescueStatus = RescueStatus.NothingToRescue;

                Assert.DoesNotThrow(() =>
                {
                    rescueStatus = rescuer.MonitorAndRescue();
                });

                Assert.AreEqual(RescueStatus.NothingToRescue, rescueStatus);
            }
        }

        [Test]
        public void Can_Monitor_And_Rescue_StoppedService_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus()).Returns(ServiceControllerStatus.Stopped);
            shell.Setup(p => p.StartService()).Returns(true);

            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                var rescueStatus = RescueStatus.NothingToRescue;

                Assert.DoesNotThrow(() =>
                {
                    rescueStatus = rescuer.MonitorAndRescue();
                });

                Assert.AreEqual(RescueStatus.Rescued, rescueStatus);
            }
        }

        [Test]
        public void Can_Increase_Rescue_Counter_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();
            
            shell.Setup(p => p.StartService()).Returns(true);

            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.AreEqual(0, rescuer.RescueCounter, "Invalid initial RescueCounter value");

                rescuer.Rescue();

                Assert.AreEqual(1, rescuer.RescueCounter, "RescueCounter should increase after successfull rescue");
            }

        }

        [Test]
        public void Dont_Increase_Counter_After_Invalid_Rescue_Test()
        {
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(false);
            shell.Setup(p => p.ErrorLog).Returns(new List<string>());
            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                Assert.AreEqual(0, rescuer.RescueCounter, "Invalid initial RescueCounter value");
                 
                Assert.Throws<ServiceRescueException>( () => rescuer.Rescue(), "In this test, Rescue() should throw exception");
                    
                Assert.AreEqual(0, rescuer.RescueCounter, "RescueCounter be still 0 after unsuccessfull rescue");
            }
        }

        [Test]
        public void Can_Get_ConnectedService_Name_Test()
        {

            var serviceName = "Test";
            var builder = new ContainerBuilder();
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.ConnectToService(It.IsAny<string>())).Returns(true);
                        
            builder.RegisterInstance(shell.Object);
            builder.RegisterType<WindowsServiceRescuer>().AsImplementedInterfaces();


            using (var container = builder.Build())
            {
                var rescuer = container.Resolve<IWindowsServiceRescuer>();

                rescuer.Connect(serviceName);

                Assert.AreEqual(serviceName, rescuer.ConnectedServiceName);
            }
        }                
    }
}