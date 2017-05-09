using System;
using System.IO;
using System.ServiceProcess;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class InstallServiceTests
    {
        private ServiceInstaller _serviceInstaller;
        private NameGenerator _nameGenerator;
        private UserInstaller _userInstaller;

        private const string Prefix = "cTestService";
        private const int AutomaticStartMode = 2;

        [SetUp]
        public void SetUp()
        {
            _serviceInstaller = new ServiceInstaller();
            _userInstaller = new UserInstaller();

            _nameGenerator = new NameGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void Install_Should_Install_When_Ok()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            // act
            _serviceInstaller.InstallService(name);

            // assert
            var serviceController = new ServiceController(name);
            serviceController.ServiceName.Should().Be(name);
        }

        [Test]
        public void Install_Should_Throw_Exectipn_When_Path_Is_Incorrect()
        {
            // arrange 
            var name = _nameGenerator.GetRandomName(Prefix);
            _serviceInstaller.ServicePath = "fakePath";

            // act
            Action act =() =>  _serviceInstaller.InstallService(name);

            // assert
            act.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Install_Should_Install_When_Provide_DefaultConfig()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var serviceController = new ServiceController(name);
            serviceController.ServiceName.Should().Be(name);
        }

        [Test]
        public void Install_Should_Throw_When_Path_IsInvalid()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = "fakePath"
            };

            // act 
            Action act = () => _serviceInstaller.InstallService(config);

            // assert
            act.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Install_Should_AddCorrectParameters()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var path = ServiceHelper.GetPath(name);
            var displayName = ServiceHelper.GetDisplayName(name);
            var account = ServiceHelper.GetAccount(name);
            var starMode = ServiceHelper.GetStartMode(name);
            var serviceType = ServiceHelper.GetServiceType(name);
            var errorControl = ServiceHelper.GetErrorControl(name);
            var description = ServiceHelper.GetDescription(name);


            path.Should().Be(config.Path);
            displayName.Should().Be(config.Name);
            account.Should().Be("LocalSystem");
            starMode.Should().Be((int) config.StartMode);
            serviceType.Should().Be((int) config.Type);
            errorControl.Should().Be((int) config.ErrorControl);
            description.Should().BeNullOrEmpty();
        }

        [Test]
        public void Install_Should_Change_DisplayName_WhenProvided()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                DisplayName = name + "222"
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var displayName = ServiceHelper.GetDisplayName(name);

            displayName.Should().Be(config.DisplayName);
        }

        [Test]
        public void Install_Should_Change_Description_WhenProvided()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                DisplayName = name + "222",
                Description = "test service to delete"
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var description = ServiceHelper.GetDescription(name);

            description.Should().Be(config.Description);
        }


        [Test]
        public void Install_Should_Change_ErrorControl_ToCritical()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ErrorControl = WindowsServiceErrorControl.Critical
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int) config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ErrorControl_ToIgnore()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ErrorControl = WindowsServiceErrorControl.Ignore
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }


        [Test]
        public void Install_Should_Change_ErrorControl_ToNormal()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ErrorControl = WindowsServiceErrorControl.Normal
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ErrorControl_ToSevere()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                ErrorControl = WindowsServiceErrorControl.Severe
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ServiceType_To_ShareProcess()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                Type = WindowsServiceType.ShareProcess
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetServiceType(name);

            errorControl.Should().Be((int)config.Type);
        }

        //todo: add tests for service types: drivers, interactive proccess 

        [Test]
        public void Install_Should_Change_StartMode_To_Disabled()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            const int disabledStartMode = 4;

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.Disabled
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

            startMode.Should().Be(disabledStartMode);
        }

        [Test]
        public void Install_Should_Change_StartMode_To_Manual()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            const int manualStartMode = 3;
            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.Manual
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

            startMode.Should().Be(manualStartMode);
        }

        [Test]
        public void Install_Should_Change_StartMode_To_Automatic()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.Automatic
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

            startMode.Should().Be((int)config.StartMode);
        }

        //todo: add test for start mode: boot, system


        [Test]
        public void Install_Should_Change_InteractiveWithDesktop_ToTrue()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                InteractWithDesktop = true
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var type = ServiceHelper.GetServiceType(name);
            var interactiveWithDekstopFlag = 0x100;
            
            type.Should().Be((int) config.Type | interactiveWithDekstopFlag);
        }

        [Test]
        public void Install_Should_Add_ChosenAccount()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = serviceName + "User";
            var userPassword = "test";

            _userInstaller.Install(userName, userPassword);

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                AccountName = userName,
                AccountPassword = userPassword
            };

            // act
            _serviceInstaller.InstallService(config);

            // assert
            var account = ServiceHelper.GetAccount(serviceName);
            account.Should().Be($".\\{userName}");
        }

        [Test]
        public void Install_Should_Throw_When_AccountIsInvalid()
        {
           // arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = "fakeUSer";
            var userPassword = "fakePassword";

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                AccountName = userName,
                AccountDomain = userPassword
            };

            // act
            Action act = () => _serviceInstaller.InstallService(config);

            // assert 
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Install_Should_Change_DelayAutostart_ToTrue()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);
            

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);
            var delayedAutostart = ServiceHelper.GetDelayedAutostart(name);

            startMode.Should().Be(AutomaticStartMode);
            delayedAutostart.Should().BeTrue();
        }
    }
}
