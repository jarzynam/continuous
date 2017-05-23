using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class ExistsByServiceExtensionTests
    {
        private NameGenerator _generator;
        private ServiceInstaller _serviceInstaller;
      
        [SetUp]
        public void SetUp()
        { 
            _serviceInstaller = new ServiceInstaller();

            _generator = new NameGenerator("testServiceEt");
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
            var service = _serviceInstaller.InstallAndGetService(_generator.RandomName);

            // Act
            var result = service.Exists();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Exists_Returns_False_If_ServiceNotExist()
        {
            // Arrange 
            var service = _serviceInstaller.InstallAndGetService(_generator.RandomName);

            _serviceInstaller.UninstallService(service.Name);

            // Act
            var result = service.Exists();

            // Assert
            result.Should().BeFalse();
        }
    }
}
