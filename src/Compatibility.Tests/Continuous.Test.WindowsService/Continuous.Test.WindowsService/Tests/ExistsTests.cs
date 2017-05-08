using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.Logic.Installer;
using Continuous.WindowsService;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public class ExistsTests
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
        public void Exists_Returns_True_If_ServiceExist()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            // Act
            var result = _shell.Exists(serviceName);

            // Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Exists_Returns_False_If_ServiceNotExist()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // Act
            var result = _shell.Exists(serviceName);

            // Assert
            result.Should().BeFalse();
        }
    }
}
