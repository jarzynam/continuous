using System.Collections.Generic;
using System.ServiceProcess;
using Moq;
using NUnit.Framework;
using Rescuer.Management.Rescuers;
using Rescuer.Management.Rescuers.WindowsService;
using Rescuer.Management.Rescuers.WindowsService.Exceptions;
using Rescuer.Management.Rescuers.WindowsService.Shell;
using Rescuer.Management.Transit;
using System;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class WindowsServiceRescuerTests
    {
        [Test]
        public void Can_Check_StoppedStatus_WindowsService_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus())
                .Returns(ServiceControllerStatus.Stopped);

            IRescuer rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            var healthStatus = rescuer.CheckHealth();

            // assert
            Assert.IsNotNull(healthStatus);
            Assert.AreEqual(HealthStatus.Stopped, healthStatus);

        }

        [Test]
        public void Can_Check_WorkingStatus_WindowsService_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus())
                .Returns(ServiceControllerStatus.Running);

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            var healthStatus = rescuer.CheckHealth();

            // assert
            Assert.IsNotNull(healthStatus);
            Assert.AreEqual(HealthStatus.Working, healthStatus);

        }

        [Test]
        public void Can_Connect_ToService_Test()
        {
            // arrange
            var windowsServiceShell = new Mock<IWindowsServiceShell>();
            windowsServiceShell.Setup(p => p.ConnectToService(It.IsAny<string>()))
                .Returns(true);

            var rescuer = new WindowsServiceRescuer(windowsServiceShell.Object);

            // act
            TestDelegate act = () => rescuer.Connect("test");

            // assert
            Assert.DoesNotThrow(act);
        }

        [Test]
        public void Can_Handle_Connection_To_NoExistingService_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.ConnectToService(It.IsAny<string>()))
                .Returns(false);
            shell.Setup(p => p.ErrorLog).Returns(new List<string> { "Service connection exception" });

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act          
            TestDelegate act = () => rescuer.Connect("test");

            // assert
            Assert.Catch<ServiceConnectionException>(act);

        }

        [Test]
        public void Can_HitServiceStatus_WhenCheckingHealth_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus())
                .Returns(ServiceControllerStatus.Running)
                .Verifiable();

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act 
            rescuer.CheckHealth();

            // assert
            shell.Verify(p => p.GetServiceStatus());
        }

        [Test]
        public void Can_Rescue_Service_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(true);

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            TestDelegate act = () => rescuer.Rescue();

            // assert
            Assert.DoesNotThrow(act);
        }

        [Test]
        public void Can_Handle_FailedResuce_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(false);
            shell.Setup(p => p.ErrorLog).Returns(new List<string> { "cant start service" });

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            TestDelegate act = () => rescuer.Rescue();

            // assert
            Assert.Throws<ServiceRescueException>(act);
        }

        [Test]
        public void Can_Monitor_And_Rescue_RunningService_Test()
        {
            // arrange
            var rescueStatus = RescueStatus.NothingToRescue;
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus()).Returns(ServiceControllerStatus.Running);

            var rescuer = new WindowsServiceRescuer(shell.Object);
            // act
            TestDelegate act = () => { rescueStatus = rescuer.MonitorAndRescue(); };

            // assert
            Assert.DoesNotThrow(act);
            Assert.AreEqual(RescueStatus.NothingToRescue, rescueStatus);
        }

        [Test]
        public void Can_Monitor_And_Rescue_StoppedService_Test()
        {
            // arrnage
            var rescueStatus = RescueStatus.NothingToRescue;
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.GetServiceStatus()).Returns(ServiceControllerStatus.Stopped);
            shell.Setup(p => p.StartService()).Returns(true);

            var rescuer = new WindowsServiceRescuer(shell.Object);

            //act
            TestDelegate act = () => { rescueStatus = rescuer.MonitorAndRescue(); };
            // assert

            Assert.DoesNotThrow(act);

            Assert.AreEqual(RescueStatus.Rescued, rescueStatus);
        }
    
        [Test]
        public void Can_Increase_Rescue_Counter_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();
            
            shell.Setup(p => p.StartService()).Returns(true);

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            var initialRescueCounter = rescuer.RescueCounter;
            rescuer.Rescue();

            var actualRescueCounter = rescuer.RescueCounter;

            // assert
            Assert.AreEqual(0, initialRescueCounter);
            Assert.AreEqual(1, actualRescueCounter);
        }

        [Test]
        public void Dont_Increase_Counter_After_Invalid_Rescue_Test()
        {
            // arrange
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.StartService()).Returns(false);
            shell.Setup(p => p.ErrorLog).Returns(new List<string>());

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            var initalRescueCounter = rescuer.RescueCounter;
            TestDelegate act = () => rescuer.Rescue();
            
            // assert
            Assert.AreEqual(0, initalRescueCounter);   
            Assert.Throws<ServiceRescueException>(act);
            Assert.AreEqual(0, rescuer.RescueCounter, "RescueCounter be still 0 after unsuccessfull rescue");
        }

        [Test]
        public void Can_Get_ConnectedService_Name_Test()
        {
            // arrange
            var serviceName = "Test";
            var shell = new Mock<IWindowsServiceShell>();

            shell.Setup(p => p.ConnectToService(It.IsAny<string>())).Returns(true);

            var rescuer = new WindowsServiceRescuer(shell.Object);

            // act
            rescuer.Connect(serviceName);

            // assert
            Assert.AreEqual(serviceName, rescuer.ConnectedServiceName);
            
        }                
    }
}