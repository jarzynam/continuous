using System;
using Autofac;
using Autofac.Features.ResolveAnything;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;


namespace Rescuer.Management.Tests
{
    [TestFixture]
    public class ManagementWindowsServiceTests
    {
        private ContainerBuilder _builder;

        [SetUp]
        public void Config()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<RescuerFactory>().As<IRescuerFactory>();

        }


        [Test]
        public void Can_Inject_MockedShell_To_WindwosServiceRescuer()
        {
            var windowsServiceShell = new Mock<IWindowsServiceShell>();
            int shellHits = 0;
            windowsServiceShell.Setup(p => p.GetServiceStatus(String.Empty))
                .Callback(() => { shellHits++; })
                .Returns("working");

            using (var container = _builder.Build())
            {
                var factory = container.Resolve<IRescuerFactory>();
                var rescuer = factory.Create("rescuerTestService", RescuerType.WindowsService);

                rescuer.CheckHealth();

                Assert.IsTrue(shellHits > 0, "Windows service rescuer should hit mocked shell at least once.");
            }
        }                      

        [Test]
        public void Can_Check_WorkingStatus_WindowsService_Test()
        {
            string serviceName = "RescuerTestService";

            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.GetServiceStatus(It.IsAny<string>()))
                .Returns("working");                

            _builder.RegisterInstance(windowsServiceShell).As<IWindowsServiceShell>();

            using (var container = _builder.Build())
            {                
                var factory = container.Resolve<IRescuerFactory>();

                IRescuer wsRescuer = factory.Create(serviceName, RescuerType.WindowsService);

                HealthStatus healthStatus = wsRescuer.CheckHealth();

                Assert.IsNotNull(healthStatus);
                Assert.AreEqual(HealthStatus.Working, healthStatus);
            }
        }

        [Test]
        public void Can_Check_StoppedStatus_WindowsService_Test()
        {
            string serviceName = "RescuerTestService";

            var windowsServiceShell = new Mock<IWindowsServiceShell>();

            windowsServiceShell.Setup(p => p.GetServiceStatus(It.IsAny<string>()))
                .Returns("stopped");

            _builder.RegisterInstance(windowsServiceShell).As<IWindowsServiceShell>();

            using (var container = _builder.Build())
            {
                var factory = container.Resolve<IRescuerFactory>();

                IRescuer wsRescuer = factory.Create(serviceName, RescuerType.WindowsService);

                HealthStatus healthStatus = wsRescuer.CheckHealth();

                Assert.IsNotNull(healthStatus);
                Assert.AreEqual(HealthStatus.Stopped, healthStatus);
            }
        }

        
    }
}
