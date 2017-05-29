using System;
using System.Collections.Generic;
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
        private const WindowsServiceStartMode AutomaticStartMode = WindowsServiceStartMode.Automatic;

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
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Update_Should_Update_Path_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
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

            model.DisplayName.Should().Be(name);
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Update_Should_Throw_If_Path_IsInvalid()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
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

            model.StartMode.Should().Be(WindowsServiceStartMode.Disabled);

            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Type.Should().Be(configForCreate.Type);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Update_Should_Update_Type_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
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

            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Update_Should_Update_ErrorControl_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
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
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Update_Should_Update_InteractWithDekstop_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
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
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
            model.Description.Should().BeNullOrEmpty();
        }


        [Test]
        public void Update_Should_Update_Description_Only()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
               
            };

            _serviceInstaller.InstallService(configForCreate);
           
            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                Description = "test service to delete"
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.Description.Should().Be(configForUpdate.Description);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_DelayedAutostart_Only_ToTrue()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            ServiceHelper.GetDelayedAutostart(name).Should().BeTrue();

            model.Description.Should().Be(configForCreate.Description);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_DelayedAutostart_Only_ToFalse()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                StartMode = WindowsServiceStartMode.Automatic
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            ServiceHelper.GetDelayedAutostart(name).Should().BeFalse();

            model.Description.Should().Be(configForCreate.Description);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_ServiceDependencies_Only_FromEmpty_ToCollection()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>()
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                ServiceDependencies = serviceDependencies
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetService(name);

            model.ServiceDependencies.Should().BeEquivalentTo(serviceDependencies);

            model.Description.Should().Be(configForCreate.Description);
            model.ErrorControl.Should().Be(configForCreate.ErrorControl);
            model.StartMode.Should().Be(AutomaticStartMode);
            model.Path.Should().Be(configForCreate.Path);
            model.DisplayName.Should().Be(name);
            model.Account.Should().Be("LocalSystem");
        }

        [Test]
        public void Update_Should_Update_ServiceDependencies_Only_FromCollection_ToCollection()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>()
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var serviceDependencies2 = new List<string>()
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };


            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                ServiceDependencies = serviceDependencies2
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies2);
        }

        [Test]
        public void Update_Should_Update_ServiceDependencies_Only_FromCollection_ToEmpty()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>()
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var serviceDependencies2 = new List<string>()
            {
                
            };

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                ServiceDependencies = serviceDependencies2
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies2);
        }

        [Test]
        public void Update_Should_Ignore_ServiceDependencies_When_Null()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>()
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            _serviceInstaller.InstallService(configForCreate);

            var configForUpdate = new WindowsServiceConfigurationForUpdate
            {
                ServiceDependencies = null
            };

            // act
            _shell.Update(name, configForUpdate);

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies);
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
