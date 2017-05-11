using System;
using Continuous.Management;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class ExecuteCommandTests
    {
        private NameGenerator _nameGenerator;
        private ServiceInstaller _serviceInstaller;
        private IWindowsServiceShell _shell;

        private const string Prefix = "etTestService";

        [SetUp]
        public void SetUp()
        {
            _shell = new ContinuousManagementFactory().WindowsServiceShell();
            _serviceInstaller = new ServiceInstaller();

            _nameGenerator = new NameGenerator();
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
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var commandCode = 155;

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // Act
            Action act = () => _shell.ExecuteCommand(serviceName, commandCode);

            // Assert
            act.ShouldNotThrow();
        }

        [Test]
        public void ExecuteCommand_Throws_When_ServiceNotExists()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var commandCode = 155;

            // Act
            Action act = () => _shell.ExecuteCommand(serviceName, commandCode);

            // Assert
            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("Can't find service with name: "+serviceName);
        }

        [Test]
        public void ExecuteCommand_Throws_When_CommandCode_IsOutOfRange()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var commandCode = 1000;

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // Act
            Action act = () => _shell.ExecuteCommand(serviceName, commandCode);

            // Assert
            act.ShouldThrow<ArgumentException>()
                .WithMessage("Command code value must be between 128 and 256, inclusive but was: " + commandCode);
        }
    }
}
