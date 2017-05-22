using System;
using System.Collections.Generic;
using System.IO;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class UpdateServiceByServiceExtenstionTests
    {
        private ServiceInstaller _serviceInstaller;
        private UserInstaller _userInstaller;
        private NameGenerator _nameGenerator;

        private const string Prefix = "uTestService";
        private const WindowsServiceStartMode AutomaticStartMode = WindowsServiceStartMode.Automatic;

        [SetUp]
        public void SetUp()
        {
           
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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var displayName = name + "Updated";

            // act
            service.Change()
                .DisplayName(displayName)
                .Apply();    

            // assert
            var model = ServiceHelper.GetService(name);

            model.DisplayName.Should().Be(displayName);

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var path = configForCreate.Path.Replace("BasicService", "BasicService2");
            
            // act
            service.Change().Path(path).Apply();

            // assert
            var model = ServiceHelper.GetService(name);

            model.Path.Should().Be(path);

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var  path = "FakePath";
            
            // act
            Action act = () => service.Change().Path(path).Apply();

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var startMode = WindowsServiceStartMode.Disabled;

            // act
            service.Change().StartMode(startMode).Apply();

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var type = WindowsServiceType.ShareProcess;
          
            // act
            service.Change().Type(type).Apply();

            // assert
            var model = ServiceHelper.GetService(name);

            model.Type.Should().Be(type);

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change().ErrorControl(WindowsServiceErrorControl.Critical).Apply();

            // assert
            var model = ServiceHelper.GetService(name);

            model.ErrorControl.Should().Be(WindowsServiceErrorControl.Critical);

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);
            _userInstaller.Install(name, "test");

            // act
            service.Change().InteractWithDesktop(true).Apply();

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
                Path = _serviceInstaller.ServicePath
            };

            var service = _serviceInstaller.InstallAndGetService(configForCreate);
            var description = "test service to delete";

            // act
            service.Change().Description(description).Apply();

            // assert
            var model = ServiceHelper.GetService(name);

            model.Description.Should().Be(description);

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            var StartMode = WindowsServiceStartMode.AutomaticDelayedStart;

            // act
            service.Change().StartMode(StartMode).Apply();

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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change()
                .StartMode(WindowsServiceStartMode.Automatic)
                .Apply();

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
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
            };

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change().ServiceDependencies(serviceDependencies).Apply();

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
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var serviceDependencies2 = new List<string>
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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change().ServiceDependencies(serviceDependencies2).Apply();

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies2);
        }

        [Test]
        public void Update_Should_Update_ServiceDependencies_Only_FromCollection_ToEmpty()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var serviceDependencies2 = new List<string>();

            var configForCreate = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change().ServiceDependencies(serviceDependencies2).Apply();

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies2);
        }

        [Test]
        public void Update_Should_Ignore_ServiceDependencies_When_Null()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>
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

            var service = _serviceInstaller.InstallAndGetService(configForCreate);

            // act
            service.Change().ServiceDependencies(null).Apply();

            // assert
            var model = ServiceHelper.GetServiceDependencies(name);

            model.Should().BeEquivalentTo(serviceDependencies);
        }

        [Test]
        public void Update_Should_Throw_When_Service_NotExist()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            _serviceInstaller.UninstallService(serviceName);

            // act
            Action act = () => service.Change().Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Update_Should_Update_AllFields_When_Ok()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var installConfig = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath
            };

            var service = _serviceInstaller.InstallAndGetService(installConfig);

            var path = _serviceInstaller.ServicePath.Replace("BasicService", "BasicService2");
            var displayName = "new displayName";
            var errorControl = WindowsServiceErrorControl.Critical;
            var startMode = WindowsServiceStartMode.Disabled;
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };
            var description = "test service to delete";
            var type = WindowsServiceType.ShareProcess;

            // act
            service.Change()
                .ServiceDependencies(serviceDependencies)
                .Description(description)
                .DisplayName(displayName)
                .ErrorControl(errorControl)
                .Path(path)
                .StartMode(startMode)
                .Type(type)
                .Apply();

            // assert
            var model = ServiceHelper.GetService(serviceName);

            model.ServiceDependencies.Should().BeEquivalentTo(serviceDependencies);

            model.Description.Should().Be(description);
            model.DisplayName.Should().Be(displayName);
            model.ErrorControl.Should().Be(errorControl);
            model.Path.Should().Be(path);
            model.StartMode.Should().Be(startMode);
            model.Type.Should().Be(type);
        }

        [Test]
        public void Update_Should_Rollback_If_Error_Occur()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var installConfig = new WindowsServiceConfiguration
            {
                Name = serviceName,
                DisplayName = serviceName,
                Path = _serviceInstaller.ServicePath
            };

            var service = _serviceInstaller.InstallAndGetService(installConfig);

            var path = "fakePath";


            var displayName = "new displayName";
            var errorControl = WindowsServiceErrorControl.Critical;
            var startMode = WindowsServiceStartMode.Disabled;
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };
            var description = "test service to delete";
            var type = WindowsServiceType.ShareProcess;

            // act
            Action act = () => service.Change()
                .ServiceDependencies(serviceDependencies)
                .Description(description)
                .DisplayName(displayName)
                .ErrorControl(errorControl)
                .Path(path)
                .StartMode(startMode)
                .Type(type)
                .InteractWithDesktop(true)
                .AccountName("fakeAccount")
                .AccountDomain("fakeDomain")
                .RollbackOnError()
                .Apply();

            act.ShouldNotThrow<FileNotFoundException>();

            // assert
            var model = ServiceHelper.GetService(serviceName);

            model.ServiceDependencies.Should().BeNullOrEmpty();

            model.Description.Should().Be(installConfig.Description);
            model.DisplayName.Should().Be(installConfig.DisplayName);
            model.ErrorControl.Should().Be(installConfig.ErrorControl);
            model.Path.Should().Be(installConfig.Path);
            model.StartMode.Should().Be(installConfig.StartMode);
            model.Type.Should().Be(installConfig.Type);
            model.Account.Should().Be("LocalSystem"); 
        }
    }
}
