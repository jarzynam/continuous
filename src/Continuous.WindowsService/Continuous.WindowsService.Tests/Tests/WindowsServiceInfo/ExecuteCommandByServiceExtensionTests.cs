using System;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class ExecuteCommandByServiceExtensionTests
    {
        private NameGenerator _generator;
        private ServiceInstaller _serviceInstaller;
      
        private const string Prefix = "etTestService";

        [SetUp]
        public void SetUp()
        {
            _serviceInstaller = new ServiceInstaller();
            _generator = new NameGenerator(Prefix);
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
        }

        [Test]
        public void ExecuteCommand_NotThrow_When_Ok()
        {
            // Arrange 
          
            var commandCode = 155;

            var service =  _serviceInstaller.InstallAndGetService(_generator.RandomName);
            ServiceHelper.StartService(service.Name);

            // Act
            Action act = () => service.ExecuteCommand(commandCode);

            // Assert
            act.ShouldNotThrow();
        }


        [Test]
        public void ExecuteCommand_Throws_When_CommandCode_IsOutOfRange()
        {
            // Arrange 
            var serviceName = _generator.GetRandomName(Prefix);
            var commandCode = 1000;

            var service = _serviceInstaller.InstallAndGetService(_generator.RandomName);
            ServiceHelper.StartService(service.Name);

            // Act
            Action act = () => service.ExecuteCommand(commandCode);

            // Assert
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Command code value must be between 128 and 256, inclusive but was: " + commandCode);
        }
    }
}
