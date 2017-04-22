using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.Logic.Installer;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public class UpdateServiceTests
    {
        private IWindowsServiceShell _shell;
        private ServiceInstaller _serviceInstaller;
        private UserInstaller _userInstaller;
        private NameGenerator _nameGenerator;

        private const string Prefix = "updateServiceTest";

        [SetUp]
        public void SetUp()
        {
            _shell = (new ContinuousManagementFactory()).WindowsServiceShell();
            _nameGenerator = new NameGenerator();

            _serviceInstaller = new ServiceInstaller();
            _userInstaller = new UserInstaller();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void Update_Can_Update_DisplayName()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path =  _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                DisplayName = name + "Updated",
                Type = configForCreate.Type,
                ErrorControl = configForCreate.ErrorControl,
                Path = configForCreate.Path,
                StartMode = configForCreate.StartMode
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var displayName = ServiceHelper.GetDisplayName(name);

            displayName.Should().Be(configForUpdate.DisplayName);
        }

    }
}
