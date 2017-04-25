using System;
using System.IO;
using Continuous.Management;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class UpdateServiceTests
    {
        private IWindowsServiceShell _shell;
        private ServiceInstaller _serviceInstaller;
        private UserInstaller _userInstaller;
        private NameGenerator _nameGenerator;

        private const string Prefix = "uTestService";

        [SetUp]
        public void SetUp()
        {
            _shell = (new ContinuousManagementFactory()).WindowsServiceShell();
            _nameGenerator = new NameGenerator();

            _serviceInstaller = new ServiceInstaller();
            _userInstaller = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void Update_Should_Update_DisplayName_Only()
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
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.DisplayName.Should().Be(configForUpdate.DisplayName);

            model.Path.Should().Be(configForCreate.Path);
            model.StartMode.Should().Be(configForCreate.StartMode);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_Path_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                Path = configForCreate.Path.Replace("BasicService", "BasicService2")
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.Path.Should().Be(configForUpdate.Path);

            model.DisplayName.Should().Be(configForCreate.DisplayName);
            model.StartMode.Should().Be(configForCreate.StartMode);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Throw_If_Path_IsInvalid()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                Path = "FakePath"
            };

            // act
            Action act = () =>  _shell.Update(name, configForUpdate);

            // assert
            act.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Update_Should_Update_StartMode_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                StartMode = WindowsServiceStartMode.Disabled
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.StartMode.Should().Be(configForUpdate.StartMode);

            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(configForCreate.DisplayName);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_Type_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                Type = WindowsServiceType.ShareProcess
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.Type.Should().Be(configForUpdate.Type);

            model.StartMode.Should().Be(configForCreate.StartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(configForCreate.DisplayName);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_ErrorControl_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                ErrorControl = WindowsServiceErrorControl.Ignore
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.ErrorControl.Should().Be(configForUpdate.ErrorControl);

            model.Type.Should().Be(configForCreate.Type);
            model.StartMode.Should().Be(configForCreate.StartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(configForCreate.DisplayName);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_InteractWithDekstop_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                DisplayName = name,
                Path = _serviceInstaller.ServicePath
            };

            _serviceInstaller.InstallService(configForCreate);
            _userInstaller.Install(name, "test");

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                InteractWithDesktop = true
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);
            var interactiveWithDekstopFlag = 0x100;

            ((int) model.Type).Should().Be((int)configForCreate.Type.GetValueOrDefault() | interactiveWithDekstopFlag);

            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.StartMode.Should().Be(configForCreate.StartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(configForCreate.DisplayName);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Throw_When_Service_NotExist()
        {
            // arrange
            var service = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.Update(service, new WindowsServiceConfigurationForUpdate());

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
