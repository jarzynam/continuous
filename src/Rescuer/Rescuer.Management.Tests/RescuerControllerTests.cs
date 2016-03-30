using System;
using System.Collections.Generic;
using Autofac;
using Moq;
using NUnit.Framework;
using Rescuer.Management.Controller;
using Rescuer.Management.Rescuers.WindowsService.Shell;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class RescuerControllerTests
    {
        [Test]
        public void Can_Initialize_WindowsServiceRescuers_Test()
        {
            var connectedServices = new List<string>();

            var shell = new Mock<IWindowsServiceShell>();
            shell.Setup(p => p.ConnectToService(It.IsAny<string>())).Returns(true).Callback((string serviceName) => {  connectedServices.Add(serviceName); });

            var builder  = new ContainerBuilder();

            builder.RegisterModule<RescuerManagementModule>();
            builder.RegisterInstance(shell.Object).As<IWindowsServiceShell>();

            using(var scope = builder.Build().BeginLifetimeScope())
            {
                var factory = new RescuerControllerFactory(scope);
                var controller = factory.Create();

                Assert.AreEqual(0, connectedServices.Count, "there shoudn't be any connected services before test started");

                controller.IntializeRescuers(new[] {"r1", "r2", "r3"});

                Assert.AreEqual("r1", connectedServices[0], "invalid first service name, where shell should try to connect");
                Assert.AreEqual("r2", connectedServices[1], "invalid second service name, where shell should try to connect");
                Assert.AreEqual("r3", connectedServices[2], "invalid third service name, where shell should try to connect");
                
            }
        }

        [Test]
        public void Can_Handle_Empty_ServiceNames_Test()
        {
            var shell = new Mock<IWindowsServiceShell>();
            shell.Setup(p => p.ConnectToService(It.IsAny<string>())).Returns(true);

            var builder = new ContainerBuilder();

            builder.RegisterModule<RescuerManagementModule>();
            builder.RegisterInstance(shell.Object).As<IWindowsServiceShell>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var factory = new RescuerControllerFactory(scope);
                var controller = factory.Create();

                Assert.Throws<ArgumentException>(() => controller.IntializeRescuers(new []{"ttt", "", "aaa"}));
            }
        }
        
        
    }
}
